using DarkRift;
using DarkRift.Client;
using Game.Scripts.GameSettings;
using Game.Scripts.Network.DarkRiftTags;
using Game.Scripts.Network.Data;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Server.Scripts
{
    public class ServerSetup : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private void Awake()
        {
            if (MasterClient.Instance.Connected)
            {
                using (var msg = Message.CreateEmpty(GameServerTags.ServerAvailable))
                {
                    MasterClient.Instance.SendMessage(msg, SendMode.Reliable);
                }

                ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
                {
                    _text.text = "Successfully connected, waiting for games.";
                });
            }
            else
            {
                if (MasterClient.Instance.Connect())
                {
                    // Register as a game server, TODO: Some kind of secret to make sure only your gameservers can register as such
                    using (var msg = Message.CreateEmpty(GameServerTags.RegisterServer))
                    {
                        MasterClient.Instance.SendMessage(msg, SendMode.Reliable);
                    }
                }
                else
                {
                    Debug.Log("Failed to connect to main server.");
                    Application.Quit();
                    return;
                }
            }

            MasterClient.Instance.MessageReceived += OnDataHandler;
            MasterClient.Instance.Disconnected += OnDisconnect;
        }

        private void OnDestroy()
        {
            if (MasterClient.Instance != null)
            {
                MasterClient.Instance.MessageReceived -= OnDataHandler;
            }
        }

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.GameServer || message.Tag >= Tags.TagsPerPlugin * (Tags.GameServer + 1))
                    return;

                // Successful register
                switch (message.Tag)
                {
                    case GameServerTags.RegisterServer:
                    {
                        try
                        {
                            using (var reader = message.GetReader())
                            {
                                ServerManager.Instance.Port = reader.ReadUInt16();
                            }
                            ServerManager.Instance.Create();
                        }
                        catch (Exception exception)
                        {
                            Debug.Log(exception.Message + exception.StackTrace);
                            Application.Quit();
                            return;
                        }

                        ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
                        {
                            _text.text = "Waiting for games...";
                        });

                        using (var msg = Message.CreateEmpty(GameServerTags.ServerAvailable))
                        {
                            MasterClient.Instance.SendMessage(msg, SendMode.Reliable);
                        }
                        break;
                    }
                    case GameServerTags.InitializeGame:
                    {
                        GameType gameType;

                        using (var reader = message.GetReader())
                        {
                            gameType = (GameType)reader.ReadByte();
                            while (reader.Position < reader.Length)
                            {
                                var player = reader.ReadSerializable<Player>();
                                ServerManager.Instance.PendingPlayers.Add(player);
                            }
                        }

                        switch (gameType)
                        {
                            case GameType.Arena:
                                ServerManager.Instance.Server.Dispatcher.InvokeWait(() =>
                                {
                                    SceneManager.LoadScene("ServerSLA");
                                });

                                break;
                            case GameType.RunlingRun:
                                ServerManager.Instance.Server.Dispatcher.InvokeWait(() =>
                                {
                                    SceneManager.LoadScene("ServerRLR");
                                });
                                break;
                            default:
                                Debug.Log("Invalid Gamemode");
                                ServerManager.Instance.Close();
                                break;
                        }
                        break;
                    }
                }
            }
        }

        private static void OnDisconnect(object obj, DisconnectedEventArgs e)
        {
            ServerManager.Instance.Close();
        }
    }
}
