from transformers import AutoTokenizer, AutoConfig, AutoModelForCausalLM

class Pygmalion:
    tokenizer = AutoTokenizer.from_pretrained("PygmalionAI/pygmalion-1.3b", use_fast=True)
    config = AutoConfig.from_pretrained("PygmalionAI/pygmalion-1.3b", is_decoder=True)
    model = AutoModelForCausalLM.from_pretrained("PygmalionAI/pygmalion-1.3b", config=config)
    
    def __init__(self):
        self.model = self.model.half()
        self.model.to("cuda")

    def prompt(self, text:str, max_length:int) -> str:
        input_ids = self.tokenizer.encode(text, return_tensors='pt').to("cuda")
        output = self.model.generate(input_ids, max_length=max_length, do_sample=True).to("cuda")
        return self.tokenizer.decode(output[0], skip_special_tokens=True)