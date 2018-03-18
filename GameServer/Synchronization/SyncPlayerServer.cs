using DarkRift;
using DarkRift.Server;
using Game.Scripts.Network.DarkRiftTags;
using Game.Scripts.Network.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Scripts.Synchronization
{
    public class SyncPlayerServer : MonoBehaviour
    {
        [SerializeField] private GameObject _gameManagerObject;

        private IControlServer _control;

        private void Awake()
        {
            _control = _gameManagerObject.GetComponent<IControlServer>();
            ServerManager.Instance.Server.ClientManager.ClientConnected += OnClientConnected;
        }

        private void OnDestroy()
        {
            if (ServerManager.Instance != null)
            {
                ServerManager.Instance.Server.ClientManager.ClientConnected -= OnClientConnected;
            }
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnMessageReceived;
        }

        #region Network Calls

        public static void InitializePlayers()
        {
            using (var msg = Message.CreateEmpty(SyncPlayerTags.InitializePlayers))
            {
                ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
            }
        }

        public static void SpawnPlayers(List<PlayerState> playerStates)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                foreach (var playerState in playerStates)
                {
                    writer.Write(playerState);
                }

                using (var msg = Message.Create(SyncPlayerTags.SpawnPlayers, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }
        }

        public static void PlayerDied(uint playerId)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(playerId);

                using (var msg = Message.Create(SyncPlayerTags.PlayerDied, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }
        }

        public static void UpdatePlayerData(List<PlayerState> playerStates)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                foreach (var state in playerStates)
                {
                    writer.Write(state);
                }

                // TODO: Change sendmode to unreliable with jitter buffer
                using (var msg = Message.Create(SyncPlayerTags.UpdatePlayerState, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }
        }

        #endregion


        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.SyncPlayer || message.Tag >= Tags.TagsPerPlugin * (Tags.SyncPlayer + 1))
                    return;

                var client = e.Client;

                // A player sent a new Input
                if (message.Tag == SyncPlayerTags.ClickPosition)
                {
                    Vector3 clickPosition;

                    using (var reader = message.GetReader())
                    {
                        clickPosition = new Vector3(reader.ReadSingle(), 0, reader.ReadSingle());
                    }

                    ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
                    {
                        _control.PlayerManagers[client.ID].PlayerMovement.MoveToPosition(clickPosition);
                    });
                }
            }
        }
    }
}
