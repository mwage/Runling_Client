using System;
using System.Net;
using DarkRift;
using DarkRift.Client;
using DarkRift.Dispatching;
using UnityEngine;

namespace Network
{
    public class GameClient : MonoBehaviour
    {
        private DarkRiftClient _darkRiftClient;
        private Dispatcher _dispatcher;

        public uint Id => _darkRiftClient.ID;
        public bool Connected => _darkRiftClient.Connected;

        // Specifies that DarkRift should take care of multithreading and invoke all events from Unity's main thread.
        private volatile bool _invokeFromDispatcher = true;

        // Specify whether DarkRift should log all data to the console.
        private volatile bool _sniffData = false;

        // Events
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<DisconnectedEventArgs> Disconnected;

        private void Awake()
        {
            _dispatcher = new Dispatcher(true);
            _darkRiftClient = new DarkRiftClient();

            _darkRiftClient.MessageReceived += Client_MessageReceived;
            _darkRiftClient.Disconnected += Client_Disconnected;
        }

        private void Update()
        {
            _dispatcher.ExecuteDispatcherTasks();
        }

        private void OnDestroy()
        {
            Close();
        }

        private void OnApplicationQuit()
        {
            Close();
        }

        public bool Connect(IPAddress ip, ushort port, IPVersion ipVersion)
        {
            if (Connected)
                return true;

            try
            {
                _darkRiftClient.Connect(ip, port, ipVersion);

                if (Connected)
                {
                    Debug.Log("Connected to " + ip + " on port " + port + " using " + ipVersion + ".");
                    return true;
                }

                Debug.Log("Connection failed to " + ip + " on port " + port + " using " + ipVersion + ".");
                return false;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message + " - " + e.StackTrace);
                return false;
            }
        }

        public void SendMessage(Message message, SendMode sendMode)
        {
            _darkRiftClient.SendMessage(message, sendMode);
        }

        private void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //If we're handling multithreading then pass the event to the dispatcher
            if (_invokeFromDispatcher)
            {
                _dispatcher.InvokeAsync(
                    () =>
                    {
                        if (_sniffData)
                        {
                            Debug.Log("Message Received");
                        }

                        var handler = MessageReceived;
                        handler?.Invoke(sender, e);
                    }
                );
            }
            else
            {
                if (_sniffData)
                {
                    _dispatcher.InvokeAsync(() => Debug.Log("Message Received"));
                }

                var handler = MessageReceived;
                handler?.Invoke(sender, e);
            }
        }

        private void Client_Disconnected(object sender, DisconnectedEventArgs e)
        {
            //If we're handling multithreading then pass the event to the dispatcher
            if (_invokeFromDispatcher)
            {
                _dispatcher.InvokeAsync(() =>
                {
                    if (_sniffData)
                    {
                        Debug.Log("Message Received");
                    }

                    var handler = Disconnected;
                    handler?.Invoke(this, e);
                }
                );
            }
            else
            {
                if (_sniffData)
                {
                    _dispatcher.InvokeAsync(() => Debug.Log("Message Received"));
                }

                var handler = Disconnected;
                handler?.Invoke(this, e);
            }
        }

        public void Close()
        {
            _darkRiftClient.Dispose();
        }
    }
}