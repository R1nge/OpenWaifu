using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class MyTcpClient : MonoBehaviour
{
    private string _host = "192.168.1.14";
    private string _port = "12345";

    public event Action<string> OnMessageReceived;

    public async void GetMessage(string text)
    {
        var result = Task.Run(() => RequestMessage(text));
        await result;
        OnMessageReceived?.Invoke(result.Result);
    }

    private string RequestMessage(object text)
    {
        var messageReceived = false;
        var message = "";
        ForceDotNet.Force();
        var timeout = new TimeSpan(0, 0, 10);
        using (var socket = new RequestSocket())
        {
            socket.Connect($"tcp://{_host}:{_port}");
            if (socket.TrySendFrame($"{text}"))
            {
                messageReceived = socket.TryReceiveFrameString(timeout, out message);

                if (messageReceived)
                {
                    print($"Socket has received a message: {message}");
                }
            }
        }

        NetMQConfig.Cleanup();
        if (!messageReceived)
        {
            message = "Could not receive message from server!";
            print(message);
        }

        return message;
    }
}