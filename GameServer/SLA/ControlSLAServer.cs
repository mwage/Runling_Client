using Game.Scripts.GameSettings;
using Game.Scripts.Network.Data;
using Game.Scripts.Players;
using Server.Scripts.Synchronization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Server.Scripts.SLA
{
    public class ControlSLAServer : MonoBehaviour, IControlServer
    {
        public Text Text;

        public Dictionary<uint, PlayerManager> PlayerManagers { get; } = new Dictionary<uint, PlayerManager>();
        public byte CurrentLevel { get; set; } = 1;
        public GameMode GameMode { get; set; }

        private InitializeGameSLAServer _initializeGame;
        private LevelManagerSLAServer _levelManager;

        private void Awake()
        {
            _initializeGame = GetComponent<InitializeGameSLAServer>();
            _levelManager = GetComponent<LevelManagerSLAServer>();
        }

        private void Start()
        {
            ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
            {
                Text.text = "Gamemode: " + GameMode;
            });

            foreach (var player in ServerManager.Instance.Players.Values)
            {
                PlayerManagers[player.Id] = _initializeGame.InitializePlayer(player);
            }
            SyncPlayerServer.InitializePlayers();

            InitializeLevel();
        }

        public void InitializeLevel()
        {
            ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
            {
                StartCoroutine(PrepareLevel());
            });
        }

        private IEnumerator PrepareLevel()
        {
            // Show Highscore and current level
            SyncGameServer.PrepareLevel(CurrentLevel);
            yield return new WaitForSeconds(3);
            SyncGameServer.HidePanels();
            yield return new WaitForSeconds(1);

            ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
            {
                Text.text = "Level " + CurrentLevel;
            });

            // Spawn Players and Drones
            var playerStates = new List<PlayerState>();

            foreach (var playerManager in PlayerManagers.Values)
            {
                playerStates.Add(_initializeGame.SpawnPlayer(playerManager));
            }
    
            SyncPlayerServer.SpawnPlayers(playerStates);

            _levelManager.LoadDrones(CurrentLevel);

            yield return new WaitForSeconds(1);

            // Countdown
            for (ushort i = 3; i > 0; i--)
            {
                SyncGameServer.Countdown(i);
                yield return new WaitForSeconds(1);
            }
            SyncGameServer.Countdown(0);

            _initializeGame.StartLevel();
        }
    }
}
