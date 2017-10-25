using System.Linq;
using DarkRift;
using DarkRift.Server;
using Network;
using Network.DarkRiftTags;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Server.Scripts.SLA
{
    public class NetworkSLAServer : MonoBehaviour
    {
        [SerializeField] private GameObject _voting;
        [SerializeField] private Text _text;

        private void Awake()
        {
            MainClient.Instance.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.ServerReady, new DarkRiftWriter()), SendMode.Reliable);

            ServerManager.Instance.Server.ClientManager.ClientConnected += OnClientConnected;
            ServerManager.Instance.Server.ClientManager.ClientDisconnected += OnClientDisconnected;
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnMessageReceived;
        }

        private void OnDestroy()
        {
            ServerManager.Instance.Server.ClientManager.ClientConnected -= OnClientConnected;
            ServerManager.Instance.Server.ClientManager.ClientDisconnected -= OnClientDisconnected;
        }

        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            // TODO: Add Player Disconnect Logic
            ServerManager.Instance.Players.Remove(e.Client);

            if (ServerManager.Instance.Players.Count == 0)
            {
                ServerManager.Instance.Server.Dispatcher.InvokeWait(() =>
                {
                    SceneManager.LoadScene("ServerSetup");
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
                ServerManager.Instance.PendingPlayers.Remove(player);
                ServerManager.Instance.Players[client] = player;

                client.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.IdentifyPlayer, new DarkRiftWriter()), SendMode.Reliable);

                ServerManager.Instance.Server.Dispatcher.InvokeWait(() =>
                {
                    _text.text = "Identified, remaining: " + ServerManager.Instance.PendingPlayers.Count;

                    if (ServerManager.Instance.PendingPlayers.Count == 0)
                    {
                        _voting.SetActive(true);
                    }
                });
            }
        }
    }
}
