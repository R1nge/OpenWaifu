import re
import asyncio
from server import Server
from nllb import Translator
from pygmalion import Pygmalion
from emotion import Emotion_Analyzer
from text_formatter import Text_Formater

MAX_LENGTH = 250

translator = Translator()
pygmalion = Pygmalion()
emotion_analyzer = Emotion_Analyzer()
text_formatter = Text_Formater()

server = Server("*", "12345")

async def main():
    print("Server started")
    while True:
        request = server.receive()
        text_string = request.decode()
        re.sub(r'(\r\n.?)+', r'\r\n', text_string)
        print(emotion_analyzer.analyze(text_string))
        await translate(text_string, "jpn_Jpan")
        
async def translate(text:str, target_language:str):
    translation = await translator.translate(text, target_language, MAX_LENGTH)
    server.send(translation)
    pass
    
asyncio.run(main())