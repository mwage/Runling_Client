using System;
using System.Net;
using DarkRift;
using DarkRift.Client;
using DarkRift.Dispatching;
using UnityEngine;

namespace Network
{
    public sealed class Client : MonoBehaviour
    {
        public IPAddress Address { get; set; } = IPAddress.Loopback;
        public ushort Port { get; set; } = 4296;
        public IPVersion IpVersion { get; set; } = IPVersion.IPv4;
        public uint ID => DarkRiftClient.ID;
        public bool Connected => DarkRiftClient.Connected;
        public DarkRiftClient DarkRiftClient { get; private set; }
        public Dispatcher Dispatcher { get; private set; }

        // Specifies that DarkRift should take care of multithreading and invoke all events from Unity's main thread.
        private volatile bool _invokeFromDispatcher = true;

        // Specify whether DarkRift should log all data to the console.
        private volatile bool _sniffData = false;

        // Events
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<DisconnectedEventArgs> Disconnected;

        private void Awake()
        {
            Dispatcher = new Dispatcher(true);
            DarkRiftClient = new DarkRiftClient();

            DarkRiftClient.MessageReceived += Client_MessageReceived;
            DarkRiftClient.Disconnected += Client_Disconnected;
        }

        private void Update()
        {
            Dispatcher.ExecuteDispatcherTasks();
        }

        private void OnDestroy()
        {
            Close();
        }

        private void OnApplicationQuit()
        {
            Close();
        }

        public bool Connect(IPAddress ip, int port, IPVersion ipVersion)
        {
            try
            {
                DarkRiftClient.Connect(ip, port, ipVersion);

                if (Connected)
                {
                    Debug.Log("Connected to " + ip + " on port " + port + " using " + ipVersion + ".");
                    return true;
                }
                else
                {
                    Debug.Log("Connection failed to " + ip + " on port " + port + " using " + ipVersion + ".");
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message + " - " + e.StackTrace);
                return false;
            }
        }

        // Connect to a remote asynchronously.
        public void ConnectInBackground(IPAddress ip, int port, IPVersion ipVersion,
            DarkRiftClient.ConnectCompleteHandler callback = null)
        {
            DarkRiftClient.ConnectInBackground(ip, port, ipVersion,
                delegate(Exception e)
                {
                    if (callback != null)
                    {
                        if (_invokeFromDispatcher)
                        {
                            Dispatcher.InvokeAsync(() => callback(e));
                        }
                        else
                        {
                            callback.Invoke(e);
                        }
                    }

                    Dispatcher.InvokeAsync(
                        delegate
                        {
                            if (Connected)
                            {
                                Debug.Log("Connected to " + ip + " on port " + port + " using " + ipVersion + ".");
                            }
                            else
                            {
                                Debug.Log("Connection failed to " + ip + " on port " + port + " using " + ipVersion +
                                          ".");
                            }
                        }
                    );
                }
            );
        }

        public void SendMessage(Message message, SendMode sendMode)
        {
            DarkRiftClient.SendMessage(message, sendMode);
        }

        private void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //If we're handling multithreading then pass the event to the dispatcher
            if (_invokeFromDispatcher)
            {
                Dispatcher.InvokeAsync(
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
                    Dispatcher.InvokeAsync(() => Debug.Log("Message Received"));
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
                Dispatcher.InvokeAsync(() =>
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
                    Dispatcher.InvokeAsync(() => Debug.Log("Message Received"));
                }

                var handler = Disconnected;
                handler?.Invoke(this, e);
            }
        }

        public void Close()
        {
            DarkRiftClient.Dispose();
        }
    }
}