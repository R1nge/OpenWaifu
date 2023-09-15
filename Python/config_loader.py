import re
import os

class ConfigLoader:

    configs_path = os.path.dirname(os.getcwd()) + "/Configs"

    def get_network_config(self) -> tuple:
        server_config_path = self.configs_path + "/ServerConfig.txt"
        server_config = open(server_config_path, "r")
        line = server_config.readline()

        re_ip = re.compile("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")
        re_port = re.compile("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\:(\d+)")

        ip = re.findall(re_ip, line)
        port = re.findall(re_port, line)

        return (ip[0], port[0])
    
    def get_voicevox_config(self) -> tuple:
        voicevox_config_path = self.configs_path + "/VoiceVoxConfig.txt"
        voicevox_config = open(voicevox_config_path, "r")
        line = voicevox_config.readline()

        re_ip = re.compile("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")
        re_port = re.compile("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\:(\d+)")

        ip = re.findall(re_ip, line)
        port = re.findall(re_port, line)

        return (ip[0], port[0])

