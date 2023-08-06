using System.IO;

public class ConfigLoader
{
    public (string, int) LoadNetworkConfig()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var parentDirectory = Directory.GetParent(currentDirectory).Parent.FullName;
        var filePath = parentDirectory + "Configs/ServerConfig";
        string[] lines = File.ReadAllLines(filePath);
        string[] bits = lines[0].Split(':');
        string host = bits[0];
        int port = int.Parse(bits[1]);
        return (host, port);
    }      
}