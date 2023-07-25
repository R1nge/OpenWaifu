from server import Server
import time
import nllb
import asyncio
import torch
from transformers import AutoModelForSeq2SeqLM, AutoTokenizer

tokenizer = AutoTokenizer.from_pretrained("facebook/nllb-200-distilled-600M", src_lang="eng_Latn")
model = AutoModelForSeq2SeqLM.from_pretrained("facebook/nllb-200-distilled-600M",
                                            torch_dtype=torch.float16,
                                            device_map= "auto",
                                            low_cpu_mem_usage=True,
                                            #load_in_8bit= True
                                            )

translator = nllb.Translator(tokenizer, model)
translator.init()

server = Server("*", "12345")
server.init()

async def main():
    print("Server started")
    while True:
        req = server.receive()
        await translate(req, "jpn_Jpan")
        
async def translate(text, target_language):
    text_string = text.decode()
    translation = await translator.translate(text_string, target_language)
    server.send(translation)
    
asyncio.run(main())