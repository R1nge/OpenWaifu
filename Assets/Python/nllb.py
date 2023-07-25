class Translator:
        def __init__(self, tokenizer, model):
                self.tokenizer = tokenizer
                self.model = model

        def init(self):
                self.model = self.model.half()
                self.model.to("cuda")

        async def translate(self, text, target_language):
                inputs = self.tokenizer(text, return_tensors="pt").to("cuda")

                translated_tokens = self.model.generate(
                        **inputs, forced_bos_token_id=self.tokenizer.lang_code_to_id[target_language], max_length=500
                )
                return self.tokenizer.batch_decode(translated_tokens, skip_special_tokens=True)[0]