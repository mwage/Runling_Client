using DarkRift;
using DarkRift.Server;
using Game.Scripts.GameSettings;
using Game.Scripts.Network.DarkRiftTags;
using Game.Scripts.Network.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Server.Scripts.SLA
{
    public class SetupSLAServer : MonoBehaviour
    {
        [SerializeField] private GameObject _voting;
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _syncManager;
        

        ////////////////////////////// For faster testing only! /////////////////////////
        private bool _testMode;
        /////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            ////////////////////////////// For faster testing only! /////////////////////////
            _testMode = false;
            
            if (_testMode)
            {
                if (ServerManager.Instance.Server == null)
                {
                    ServerManager.Instance.Create();
                }
                ServerManager.Instance.Server.Dispatcher.InvokeWait(() =>
                {
                    _syncManager.SetActive(true);
                });
            }
            /////////////////////////////////////////////////////////////////////////////////
            else
            {
                ServerManager.Instance.Server.Dispatcher.InvokeWait(() =>
                {
                    _syncManager.SetActive(true);
                });
                using (var msg = Message.CreateEmpty(GameServerTags.ServerReady))
                {
                    MasterClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }

            ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
            {
                _text.text = "Connected: 0/" + ServerManager.Instance.PendingPlayers.Count;
            });

            ServerManager.Instance.Server.ClientManager.ClientConnected += OnClientConnected;
            ServerManager.Instance.Server.ClientManager.ClientDisconnected += OnClientDisconnected;
        }

        private void OnDestroy()
        {
            if (ServerManager.Instance != null)
            {
                ServerManager.Instance.Server.ClientManager.ClientConnected -= OnClientConnected;
                ServerManager.Instance.Server.ClientManager.ClientDisconnected -= OnClientDisconnected;
            }
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnMessageReceived;
        }

        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            // TODO: Add Player Disconnect Logic
            ServerManager.Instance.Players.Remove(e.Client);

            if (ServerManager.Instance.Players.Count == 0)
            {
                ServerManager.Instance.Server.Dispatcher.InvokeWait(() =>
                {
                    /////////////////////////////////////////////////////////////////////
                    SceneManager.LoadScene(_testMode ? "ServerSLA" : "ServerSetup");
                    //////////////////////////////////////////////////////////////////
                });
            }
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.GameServer || message.Tag >= Tags.TagsPerPlugin * (Tags.GameServer + 1))
                    return;

                var client = e.Client;


                // Identify Player
                if (message.Tag == GameServerTags.IdentifyPlayer)
                {
                    uint mainId;
                    using (var reader = message.GetReader())
                    {
                        mainId = reader.ReadUInt32();
                    }
                    var player = ServerManager.Instance.PendingPlayers.FirstOrDefault(p => p.Id == mainId);
                    if (player == null)
                    {
                        Debug.Log("Failed to identify player");
                        using (var msg = Message.CreateEmpty(GameServerTags.IdentifyPlayerFailed))
                        {
                            client.SendMessage(msg, SendMode.Reliable);
                        }
                        return;
                    }

                    ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
                    {
                        ServerManager.Instance.PendingPlayers.Remove(player);
                        ServerManager.Instance.Players[client] = player;
                        ServerManager.Instance.Players[client].Id = client.ID;

                        foreach (var cl in ServerManager.Instance.Players.Keys)
                        {
                            if (cl == client)
                            {
                                // Let player know who's already in the game
                                using (var writer = DarkRiftWriter.Create())
                                {
                                    foreach (var pl in ServerManager.Instance.Players.Values)
                                    {
                                        writer.Write(pl);
                                    }

                                    using (var msg = Message.Create(GameServerTags.IdentifyPlayer, writer))
                                    {
                                        client.SendMessage(msg, SendMode.Reliable);
                                    }
                                }
                            }
                            else
                            {
                                // Let others know who joined
                                using (var writer = DarkRiftWriter.Create())
                                {
                                    writer.Write(player);

                                    using (var msg = Message.Create(GameServerTags.PlayerJoined, writer))
                                    {
                                        cl.SendMessage(msg, SendMode.Reliable);
                                    }
                                }
                            }
                        }

                        _text.text = "Connected: " + ServerManager.Instance.Players.Count + "/" +
                                     ServerManager.Instance.PendingPlayers.Count + ServerManager.Instance.Players.Count;

                        if (ServerManager.Instance.PendingPlayers.Count == 0)
                        {
                            _voting.SetActive(true);
                        }
                    });
                }
                ////////////////////////////// For faster testing only! /////////////////////////
                else if (message.Tag == GameServerTags.TestModeSLA)
                {
                    ServerManager.Instance.Players[client] = new Player(client.ID, "AwesomeName", PlayerColor.Green);

                    using (var writer = DarkRiftWriter.Create())
                    {
                        writer.Write(ServerManager.Instance.Players[client]);

                        using (var msg = Message.Create(GameServerTags.IdentifyPlayer, writer))
                        {
                            client.SendMessage(msg, SendMode.Reliable);
                        }
                    }

                    ServerManager.Instance.Server.Dispatcher.InvokeWait(() =>
                        {
                            _voting.SetActive(true);
                        }
                    );
                }
                ////////////////////////////////////////////////////////////////////////////////
            }
        }
    }
}
