using System;
using System.Threading.Tasks;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

namespace GPT
{
    public class MyTcpClient
    {
        public event Action<string> OnMessageReceived;
    
        private string _host;
        private string _port;
        private ConfigLoader _configLoader;

        public void Init()
        {
            _configLoader = new ConfigLoader();
            var config = _configLoader.LoadNetworkConfig();
            _host = config.Item1;
            _port = config.Item2.ToString();
        }

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
            var timeout = new TimeSpan(0, 0, 30);
        
            using (var socket = new RequestSocket())
            {
                socket.Connect($"tcp://{_host}:{_port}");
                if (socket.TrySendFrame($"{text}"))
                {
                    messageReceived = socket.TryReceiveFrameString(timeout, out message);

                    if (messageReceived)
                    {
                        Debug.Log($"Socket has received a message: {message}");
                    }
                }
            }

            NetMQConfig.Cleanup();
            if (!messageReceived)
            {
                message = "Could not receive message from server!";
                Debug.LogWarning(message);
            }

            return message;
        }
    }
}