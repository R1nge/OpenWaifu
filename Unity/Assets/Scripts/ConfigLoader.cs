using System.IO;
using UnityEngine;

public class ConfigLoader
{
    public (string, int) LoadNetworkConfig()
    {
        var persistentDataPath = Application.persistentDataPath;
        var filePath = persistentDataPath + "/Configs/ServerConfig.txt";
        string[] lines = File.ReadAllLines(filePath);
        string[] bits = lines[0].Split(':');
        string host = bits[0];
        int port = int.Parse(bits[1]);
        return (host, port);
    }
}