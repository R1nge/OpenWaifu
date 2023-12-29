import json
import torch
import docker
import asyncio
import platform
from server import Server
from emotion import Emotion_Analyzer
from config_loader import ConfigLoader
from text_formatter import Text_Formater

MAX_LENGTH = 1000

is_linux = platform.system() == "Linux"

config_loader = ConfigLoader()

run_pygmalion = input("Run pygmalion? (y/n): ").lower().strip() == "y"
if run_pygmalion:
    from pygmalion import Pygmalion
    pygmalion = Pygmalion()

run_voicevox = input("Run VoiceVox? Will run translator (y/n): ").lower().strip() == "y"
voicevox_gpu = input("Run VoiceVox on GPU? (y/n): ").lower().strip() == "y"
if run_voicevox:
    from translator import Translator
    translator = Translator()
    client = docker.from_env()

    if voicevox_gpu:
        if is_linux:
            contrainer = client.containers.run(
                'voicevox/voicevox_engine:nvidia-ubuntu20.04-latest', 
                hostname= "192.168.1.14:50021", 
                runtime="nvidia", 
                environment=["VV_CPU_NUM_THREADS=8"],
                device_requests=[docker.types.DeviceRequest(device_ids=["0"], capabilities=[['gpu']])]
                ) 
        else:
            contrainer = client.containers.run(
                'voicevox/voicevox_engine:nvidia-ubuntu20.04-latest', 
                hostname= "192.168.1.14:50021", 
                environment=["VV_CPU_NUM_THREADS=8"],
                device_requests=[docker.types.DeviceRequest(device_ids=["0"], capabilities=[['gpu']])]
                ) 
    else:
        contrainer = client.containers.run(
            'voicevox/voicevox_engine:cpu-ubuntu20.04-latest',  
            hostname= "192.168.1.14:50021",
            environment=["VV_CPU_NUM_THREADS=8"]
            )

torch.cuda.empty_cache()

emotion_analyzer = Emotion_Analyzer()
text_formatter = Text_Formater()


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