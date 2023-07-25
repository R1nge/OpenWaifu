using System;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class MyTcpClient : MonoBehaviour
{
    private string _host = "localhost";
    private string _port = "12345";

    public event Action<string> OnFinishedTranslation;

    public void RequestMessage(string text)
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
                print($"Socket has received a message: {message}");
                OnFinishedTranslation?.Invoke(message);
            }
        }

        NetMQConfig.Cleanup();
        if (!messageReceived)
        {
            message = "Could not receive message from server!";
            print(message);
        }
        //_messageCallback(message);
    }
}