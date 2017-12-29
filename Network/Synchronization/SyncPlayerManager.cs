using DarkRift;
using DarkRift.Client;
using Network.DarkRiftTags;
using Network.Synchronization.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Network.Synchronization
{
    public class SyncPlayerManager : MonoBehaviour
    {
        #region Events

        public delegate void InitializePlayersEventHandler();
        public delegate void SpawnPlayersEventHandler(List<PlayerState> playerState);
        public delegate void PlayerDeathEventHandler(uint playerId);
        public delegate void UpdatePlayersEventHandler(List<PlayerState> playerStates);

        public static event InitializePlayersEventHandler onInitializePlayers;
        public static event SpawnPlayersEventHandler onSpawnPlayers;
        public static event PlayerDeathEventHandler onPlayerDeath;
        public static event UpdatePlayersEventHandler onUpdatePlayers;

        #endregion

        private void Awake()
        {
            GameClient.Instance.MessageReceived += OnDataHandler;
        }

        #region NetworkCalls

        public static void SendClickPosition(Vector3 clickPosition)
        {
            var writer = new DarkRiftWriter();
            writer.Write(clickPosition.x);
            writer.Write(clickPosition.z);

            GameClient.Instance.SendMessage(new TagSubjectMessage(Tags.SyncPlayer, SyncPlayerSubjects.ClickPosition, writer), SendMode.Reliable);
        }

        #endregion

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.SyncPlayer)
                return;

            // Initialize Players
            if (message.Subject == SyncPlayerSubjects.InitializePlayers)
            {
                onInitializePlayers?.Invoke();
            }
            else if (message.Subject == SyncPlayerSubjects.SpawnPlayers)
            {
                var reader = message.GetReader();
                var playerStates = new List<PlayerState>();

                while (reader.Position < reader.Length)
                {
                    playerStates.Add(reader.ReadSerializable<PlayerState>());
                }

                onSpawnPlayers?.Invoke(playerStates);
            }
            else if (message.Subject == SyncPlayerSubjects.PlayerDied)
            {
                var reader = message.GetReader();
                var playerId = reader.ReadUInt32();

                onPlayerDeath?.Invoke(playerId);
            }
            else if (message.Subject == SyncPlayerSubjects.UpdatePlayerState)
            {
                var reader = message.GetReader();
                var playerStates = new List<PlayerState>();

                while (reader.Position < reader.Length)
                {
                    playerStates.Add(reader.ReadSerializable<PlayerState>());
                }
                
                onUpdatePlayers?.Invoke(playerStates);
            }
        }
    }
}
