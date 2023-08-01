import re
import json
import asyncio
from server import Server
from nllb import Translator
#from pygmalion import Pygmalion
from emotion import Emotion_Analyzer
from text_formatter import Text_Formater

MAX_LENGTH = 250

translator = Translator()
#pygmalion = Pygmalion()
emotion_analyzer = Emotion_Analyzer()
text_formatter = Text_Formater()

server = Server("*", "12345")

async def main():
    print("Server started")
    while True:
        request = server.receive()
        text_string = request.decode()
        re.sub(r'(\r\n.?)+', r'\r\n', text_string)
        translation = await translate(text_string, "jpn_Jpan")
        emotions = emotion_analyzer.analyze(text_string)
        r = {'text': translation, 'emotions': emotions}
        r = json.dumps(r, ensure_ascii=False)
        server.send_string(r)
        
async def translate(text:str, target_language:str):
    return await translator.translate(text, target_language, MAX_LENGTH)

def send_json(json:str):
    server.send_string(json)
    
asyncio.run(main())