using System;
using System.Net;
using DarkRift;
using DarkRift.Client;
using DarkRift.Dispatching;
using Game.Scripts;
using UnityEngine;

namespace Client.Scripts.Network
{
    public class MainClient : Singleton<MainClient>
    {
        protected MainClient()
        {
        }

        private readonly IPAddress _ip = IPAddress.Loopback;
        private const ushort Port = 4296;
        private const IPVersion IpVersion = IPVersion.IPv4;
        private DarkRiftClient _darkRiftClient;
        private Dispatcher _dispatcher;

        public uint Id => _darkRiftClient.ID;
        public bool Connected => _darkRiftClient.Connected;

        public IPAddress GameServerIp => _ip;
        public ushort GameServerPort { get; set; } = 4297;
        public IPVersion GameServerIpVersion => IpVersion;

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

        public override void OnDestroy()
        {
            Close();
            base.OnDestroy();
        }

        private void OnApplicationQuit()
        {
            Close();
        }

        public bool Connect()
        {
            if (Connected)
                return true;

            try
            {
                _darkRiftClient.Connect(_ip, Port, IpVersion);

                if (Connected)
                {
                    Debug.Log("Connected to " + _ip + " on port " + Port + " using " + IpVersion + ".");
                    return true;
                }

                Debug.Log("Connection failed to " + _ip + " on port " + Port + " using " + IpVersion + ".");
                return false;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message + " - " + e.StackTrace);
                return false;
            }
        }

        public void Disconnect()
        {
            _darkRiftClient.Disconnect();
        }

        // Connect to a remote asynchronously.
        public void ConnectInBackground(DarkRiftClient.ConnectCompleteHandler callback = null)
        {
            _darkRiftClient.ConnectInBackground(_ip, Port, IpVersion,
                delegate(Exception e)
                {
                    if (callback != null)
                    {
                        if (_invokeFromDispatcher)
                        {
                            _dispatcher.InvokeAsync(() => callback(e));
                        }
                        else
                        {
                            callback.Invoke(e);
                        }
                    }

                    _dispatcher.InvokeAsync(
                        delegate
                        {
                            if (Connected)
                            {
                                Debug.Log("Connected to " + _ip + " on port " + Port + " using " + IpVersion + ".");
                            }
                            else
                            {
                                Debug.Log("Connection failed to " + _ip + " on port " + Port + " using " + IpVersion +
                                          ".");
                            }
                        }
                    );
                }
            );
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
            _darkRiftClient?.Dispose();
        }
    }
}