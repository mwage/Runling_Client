using DarkRift;
using DarkRift.Server;
using Network.DarkRiftTags;
using Network.Synchronization.Data;
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

        public static void SpawnPlayers(List<PlayerState> playerStates)
        {
            // TODO: maybe group data with other players and send together, use playersync data instead of pos/id seperately)
            var writer = new DarkRiftWriter();
            foreach (var playerState in playerStates)
            {
                writer.Write(playerState);
            }

            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncPlayer, SyncPlayerSubjects.SpawnPlayers, writer), SendMode.Reliable);
        }

        public static void PlayerDied(uint playerId)
        {
            var writer = new DarkRiftWriter();
            writer.Write(playerId);

            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncPlayer, SyncPlayerSubjects.PlayerDied, writer), SendMode.Reliable);
        }

        public static void UpdatePlayerData(List<PlayerState> playerStates)
        {
            var writer = new DarkRiftWriter();
            foreach (var state in playerStates)
            {
                writer.Write(state);
            }

            // TODO: Change sendmode to unreliable
            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncPlayer, SyncPlayerSubjects.UpdatePlayerState, writer), SendMode.Reliable);
        }

        #endregion


        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;
            if (message == null || message.Tag != Tags.SyncPlayer)
                return;

            var client = (Client)sender;

            // A player sent a new Input
            if (message.Subject == SyncPlayerSubjects.ClickPosition)
            {
                var reader = message.GetReader();
                var clickPosition = new Vector3(reader.ReadSingle(), 0, reader.ReadSingle());

                ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
                {
                    _control.PlayerManagers[client.GlobalID].PlayerMovement.MoveToPosition(clickPosition);
                });
            }
        }
    }
}
