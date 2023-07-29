import torch
from transformers import AutoModelForSeq2SeqLM, AutoTokenizer

class Translator:
        tokenizer = AutoTokenizer.from_pretrained("facebook/nllb-200-distilled-600M", src_lang="eng_Latn", use_fast=True)
        model = AutoModelForSeq2SeqLM.from_pretrained("facebook/nllb-200-distilled-600M",
                                            #torch_dtype=torch.float16,
                                            #device_map= "auto",
                                            low_cpu_mem_usage=True
                                            #load_in_8bit= True
                                            )

        def __init__(self):
                #self.model = self.model.half()
                self.model.to("cpu")

        async def translate(self, text:str, target_language:str, max_length:int) -> str:
                inputs = self.tokenizer(text, return_tensors="pt").to("cpu")

                translated_tokens = self.model.generate(
                        **inputs, forced_bos_token_id=self.tokenizer.lang_code_to_id[target_language], 
                        max_length=max_length
                )
                return self.tokenizer.batch_decode(translated_tokens, skip_special_tokens=True)[0]