using System;
using System.Threading;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class MyTcpClient : MonoBehaviour
{
    private string _host = "192.168.1.14";
    private string _port = "12345";

    public event Action<string> OnFinishedTranslation;

    public void CreateNewThread(string text)
    {
        Thread requestThread = new Thread(() => { RequestMessage(text); })
        {
            Name = "Request Thread"
        };
        requestThread.Start();
    }

    private void RequestMessage(object text)
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