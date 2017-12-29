using DarkRift;
using DarkRift.Server;
using Launcher;
using Network;
using Network.DarkRiftTags;
using Network.Synchronization.Data;
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
                MainClient.Instance.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.ServerReady, new DarkRiftWriter()), SendMode.Reliable);
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
            var message = e.Message as TagSubjectMessage;
            if (message == null || message.Tag != Tags.GameServer)
                return;

            var client = (Client)sender;

            // Identify Player
            if (message.Subject == GameServerSubjects.IdentifyPlayer)
            {
                var reader = message.GetReader();
                var mainId = reader.ReadUInt32();
                var player = ServerManager.Instance.PendingPlayers.FirstOrDefault(p => p.Id == mainId);
                if (player == null)
                {
                    Debug.Log("Failed to identify player");
                    client.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.IdentifyPlayerFailed, new DarkRiftWriter()), SendMode.Reliable);
                    return;
                }

                ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
                {
                    ServerManager.Instance.PendingPlayers.Remove(player);
                    ServerManager.Instance.Players[client] = player;
                    ServerManager.Instance.Players[client].Id = client.GlobalID;

                    foreach (var cl in ServerManager.Instance.Players.Keys)
                    {
                        var writer = new DarkRiftWriter();

                        if (cl == client)
                        {
                            // Let player know who's already in the game
                            foreach (var pl in ServerManager.Instance.Players.Values)
                            {
                                writer.Write(pl);
                            }
                            client.SendMessage(
                                new TagSubjectMessage(Tags.GameServer, GameServerSubjects.IdentifyPlayer, writer),
                                SendMode.Reliable);
                        }
                        else
                        {
                            // Let others know who joined
                            writer.Write(player);
                            cl.SendMessage(
                                new TagSubjectMessage(Tags.GameServer, GameServerSubjects.PlayerJoined, writer),
                                SendMode.Reliable);
                        }
                    }

                    _text.text = "Connected: " + ServerManager.Instance.Players.Count + "/" + ServerManager.Instance.PendingPlayers.Count + ServerManager.Instance.Players.Count;

                    if (ServerManager.Instance.PendingPlayers.Count == 0)
                    {
                        _voting.SetActive(true);
                    }
                });
            }
            ////////////////////////////// For faster testing only! /////////////////////////
            if (message.Subject == GameServerSubjects.TestModeSLA)
            {
                ServerManager.Instance.Players[client] = new Player(client.GlobalID, "AwesomeName", PlayerColor.Green);

                var writer = new DarkRiftWriter();
                writer.Write(ServerManager.Instance.Players[client]);
                client.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.IdentifyPlayer, writer), SendMode.Reliable);

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
