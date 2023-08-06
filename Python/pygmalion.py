from transformers import AutoTokenizer, AutoConfig, AutoModelForCausalLM, TextStreamer, StoppingCriteria, StoppingCriteriaList

class Pygmalion:
    tokenizer = AutoTokenizer.from_pretrained("PygmalionAI/pygmalion-1.3b", use_fast=True)
    config = AutoConfig.from_pretrained("PygmalionAI/pygmalion-1.3b", is_decoder=True)
    model = AutoModelForCausalLM.from_pretrained("PygmalionAI/pygmalion-1.3b", config=config)
    
    def __init__(self):
        self.model = self.model.half()
        self.model.to("cuda")
        stop_words = ['R1nge: ', 'You: ', '<|endoftext|>']
        self.stop_ids = [self.tokenizer.encode(w)[0] for w in stop_words]
        self.stop_criteria = KeywordsStoppingCriteria(self.stop_ids)

    def prompt(self, text:str, max_length:int) -> str:
        input = self.tokenizer.encode(text, return_tensors='pt').to("cuda")
        streamer = TextStreamer(tokenizer=self.tokenizer)
        output = self.model.generate(input, max_length=max_length, do_sample=True, streamer=streamer, stopping_criteria=StoppingCriteriaList([self.stop_criteria])).to("cuda")
        text = self.tokenizer.decode(output[0], skip_special_tokens=True)
        return text
    
class KeywordsStoppingCriteria(StoppingCriteria):
    def __init__(self, keywords_ids:list):
        self.keywords = keywords_ids

    def __call__(self, input_ids, scores, **kwargs) -> bool:
        if input_ids[0][-1] in self.keywords:
            return True
        return False