using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading;

namespace SignalRReceiver
{
    public class SignalRHubConnection
    {
        HubConnection _connection = null;
        IHubProxy _myHub;
        public event EventHandler<string> MessageReceivedEvent;

        public SignalRHubConnection()
        {
        }

        public async void Connect()
        {
                _connection = new HubConnection("http://localhost:8080/");
                _myHub = _connection.CreateHubProxy("MyHub");

                _connection.Start().ContinueWith(task => {
                    if (task.IsFaulted)
                    {
                        MessageReceivedEvent(this, "error connection");

                        Console.WriteLine("There was an error opening the connection:{0}",
                                          task.Exception.GetBaseException());
                    }
                    else
                    {
                        MessageReceivedEvent(this, "Connected");

                        Console.WriteLine("Connected");
                    }

                }).Wait();

                _myHub.On<string>("addMessage", param =>
                {
                    Console.WriteLine(param);
                    MessageReceivedEvent(this, param);
                });
        }

        public void SendMessage(string message)
        {
            _myHub.Invoke<string>("Send", message).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine(task.Result);
                }
            });
        }

        public void Disconnect()
        {
            _connection.Stop();

        }
    }
}
