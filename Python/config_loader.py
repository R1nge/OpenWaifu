import re
import os

class ConfigLoader:
    def get_network_config(self) -> tuple:
        server_config_path = (os.path.dirname(os.getcwd()) + "/Configs/ServerConfig.txt")
        server_config = open(server_config_path, "r")
        lines = server_config.readlines()

        re_ip = re.compile("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")
        re_port = re.compile("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\:(\d+)")

        for line in lines:
            ip = re.findall(re_ip, line)
            port = re.findall(re_port, line)

        return (ip[0], port[0])
