using DarkRift;
using DarkRift.Client;
using Game.Scripts.Network.DarkRiftTags;
using Game.Scripts.Network.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Network.Syncronization
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
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(clickPosition.x);
                writer.Write(clickPosition.z);

                using (var msg = Message.Create(SyncPlayerTags.ClickPosition, writer))
                {
                    GameClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
        }

        #endregion

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.SyncPlayer || message.Tag >= Tags.TagsPerPlugin * (Tags.SyncPlayer + 1))
                    return;
                
                if (message.Tag == SyncPlayerTags.UpdatePlayerState)
                {
                    var playerStates = new List<PlayerState>();
                    using (var reader = message.GetReader())
                    {
                        while (reader.Position < reader.Length)
                        {
                            playerStates.Add(reader.ReadSerializable<PlayerState>());
                        }
                    }
                    onUpdatePlayers?.Invoke(playerStates);
                }
                else if (message.Tag == SyncPlayerTags.InitializePlayers)
                {
                    onInitializePlayers?.Invoke();
                }
                else if (message.Tag == SyncPlayerTags.SpawnPlayers)
                {
                    var playerStates = new List<PlayerState>();
                    using (var reader = message.GetReader())
                    {
                        while (reader.Position < reader.Length)
                        {
                            playerStates.Add(reader.ReadSerializable<PlayerState>());
                        }
                    }
                    onSpawnPlayers?.Invoke(playerStates);
                }
                else if (message.Tag == SyncPlayerTags.PlayerDied)
                {
                    using (var reader = message.GetReader())
                    {
                        var playerId = reader.ReadUInt32();
                        onPlayerDeath?.Invoke(playerId);
                    }
                }

            }
        }
    }
}
