using DarkRift;
using DarkRift.Client;
using Launcher;
using Network;
using Network.DarkRiftTags;
using Network.Synchronization;
using System;
using Network.Synchronization.Data;
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
            if (MainClient.Instance.Connected)
            {
                MainClient.Instance.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.ServerAvailable, new DarkRiftWriter()), SendMode.Reliable);


                ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
                {
                    _text.text = "Successfully connected, waiting for games.";
                });
            }
            else
            {
                if (MainClient.Instance.Connect())
                {
                    // Register as a game server
                    MainClient.Instance.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.RegisterServer, new DarkRiftWriter()), SendMode.Reliable);
                }
                else
                {
                    Debug.Log("Failed to connect to main server.");
                    Application.Quit();
                    return;
                }
            }

            MainClient.Instance.MessageReceived += OnDataHandler;
            MainClient.Instance.Disconnected += OnDisconnect;
        }

        private void OnDestroy()
        {
            if (MainClient.Instance != null)
            {
                MainClient.Instance.MessageReceived -= OnDataHandler;
            }
        }

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.GameServer)
                return;

            // Successful register
            if (message.Subject == GameServerSubjects.RegisterServer)
            {
                var reader = message.GetReader();
                try
                {
                    ServerManager.Instance.Port = reader.ReadUInt16();
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

                MainClient.Instance.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.ServerAvailable, new DarkRiftWriter()), SendMode.Reliable);
            }
            // New game
            if (message.Subject == GameServerSubjects.InitializeGame)
            {
                var reader = message.GetReader();
                var gameType = (GameType) reader.ReadByte();
                while (reader.Position < reader.Length)
                {
                    var player = reader.ReadSerializable<Player>();
                    ServerManager.Instance.PendingPlayers.Add(player);
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
            }
        }

        private static void OnDisconnect(object obj, DisconnectedEventArgs e)
        {
            ServerManager.Instance.Close();
        }
    }
}
