import torch
from transformers import AutoModelForSeq2SeqLM, AutoTokenizer

class Translator:
        run_model_on = ""
        selected_gpu = 0
        
        if(torch.cuda.is_available()):
                use_gpu = input('Translator: Use GPU? Requires 5GB VRAM (y/n): ').lower().strip() == 'y'
                if (torch.cuda.device_count() > 1):
                        for i in range(torch.cuda.device_count()):
                                print(torch.cuda.get_device_properties(i).name)
                        
                        selected_gpu = input('What GPU to use?')
        else:
                print("GPU not found, running on CPU")
        
        if use_gpu:
                run_model_on = f"cuda:{selected_gpu}"
        else:
                run_model_on = "cpu"

        tokenizer = AutoTokenizer.from_pretrained("facebook/nllb-200-distilled-600M", src_lang="eng_Latn", use_fast=True)
        model = AutoModelForSeq2SeqLM.from_pretrained("facebook/nllb-200-distilled-600M",
                                            low_cpu_mem_usage=True
                                          )

        def __init__(self):
                self.model.to(self.run_model_on)

        def translate(self, text:str, target_language:str, max_length:int) -> str:
                inputs = self.tokenizer(text, return_tensors="pt").to(self.run_model_on)

                translated_tokens = self.model.generate(
                        **inputs, forced_bos_token_id=self.tokenizer.lang_code_to_id[target_language], 
                        max_length=max_length
                )
                return self.tokenizer.batch_decode(translated_tokens, skip_special_tokens=True)[0]
