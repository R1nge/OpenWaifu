import json
import torch
import asyncio
from server import Server
from emotion import Emotion_Analyzer
from config_loader import ConfigLoader
from text_formatter import Text_Formater

MAX_LENGTH = 1000

torch.cuda.empty_cache()

run_pygmalion = input("Run pygmalion? (y/n): ").lower().strip() == "y"
if run_pygmalion:
    from pygmalion import Pygmalion
    pygmalion = Pygmalion()

torch.cuda.empty_cache()

run_transltator = input("Run translator? (y/n): ").lower().strip() == "y"
if run_transltator:
    from translator import Translator
    translator = Translator()

torch.cuda.empty_cache()

emotion_analyzer = Emotion_Analyzer()
text_formatter = Text_Formater()

config_loader = ConfigLoader()
server_config = config_loader.get_network_config()
ip = server_config[0]
port = server_config[1]

server = Server(ip, port)

async def main():
    print(f"Server started at {ip}:{port}")
    while True:
        request = server.receive()
        text_string = request.decode()
        # text_string = re.sub(r'(\r\n.?)+', r'\r\n', text_string)
        # text_string=text_string.strip('\r\n ')
        # text_string = re.sub('\s+',' ', text_string)
        # text = pygmalion.prompt(f"{personality} <START>/n What's your name?", MAX_LENGTH)
        # text = text.split("\n",1)[1]
        emotions = emotion_analyzer.analyze(text_string)
        translation = translator.translate(text_string, "jpn_Jpan", MAX_LENGTH)
        r = {"text": translation, "emotions": emotions}
        r = json.dumps(r, ensure_ascii=False)
        server.send_string(r)


asyncio.run(main())