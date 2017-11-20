using System;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using Network.DarkRiftTags;
using Network.Synchronization.Data;
using UnityEngine;

namespace Network.Synchronization
{
    public class SyncPlayerManager : MonoBehaviour
    {
        #region Events

        public delegate void SpawnPlayersEventHandler(PlayerState playerState);
        public delegate void PlayerDeathEventHandler(uint playerId);
        public delegate void UpdatePlayersEventHandler(List<PlayerState> playerStates);

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

            if (message.Subject == SyncPlayerSubjects.SpawnPlayers)
            {
                var reader = message.GetReader();
                var playerState = reader.ReadSerializable<PlayerState>();

                onSpawnPlayers?.Invoke(playerState);
            }
            else if (message.Subject == SyncPlayerSubjects.PlayerDied)
            {
                var reader = message.GetReader();
                var playerId = reader.ReadUInt32();

                onPlayerDeath?.Invoke(playerId);
            }
            else if (message.Subject == SyncPlayerSubjects.UpdatePlayerState)
            {
                var playerStates = new List<PlayerState>();
                var reader = message.GetReader();

                while (reader.Position < reader.Length)
                {
                    playerStates.Add(reader.ReadSerializable<PlayerState>());
                }
                
                onUpdatePlayers?.Invoke(playerStates);
            }
        }
    }
}
