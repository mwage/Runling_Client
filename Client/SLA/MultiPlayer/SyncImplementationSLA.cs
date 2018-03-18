using Client.Scripts.Network;
using Client.Scripts.Network.Syncronization;
using Game.Scripts.Network;
using Game.Scripts.Network.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.SLA.MultiPlayer
{
    public class SyncImplementationSLA : MonoBehaviour
    {
        [SerializeField] private InitializeGameSLA _initializeGame;

        private ControlSLA _controlSLA;
        private DeathSLA _death;

        private void Awake()
        {
            _death = _initializeGame.gameObject.GetComponent<DeathSLA>();
            _controlSLA = _initializeGame.gameObject.GetComponent<ControlSLA>();

            SyncPlayerManager.onInitializePlayers += InitializePlayers;
            SyncPlayerManager.onSpawnPlayers += SpawnPlayers;
            SyncPlayerManager.onPlayerDeath += PlayerDeath;
            SyncPlayerManager.onUpdatePlayers += UpdatePlayers;

            SyncGameManager.onCountdown += Countdown;
            SyncGameManager.onPrepareLevel += PrepareLevel;
            SyncGameManager.onStartLevel += StartLevel;
            SyncGameManager.onHidePanels += HidePanels;
        }

        private void OnDestroy()
        {
            SyncPlayerManager.onInitializePlayers -= InitializePlayers;
            SyncPlayerManager.onSpawnPlayers -= SpawnPlayers;
            SyncPlayerManager.onPlayerDeath -= PlayerDeath;
            SyncPlayerManager.onUpdatePlayers -= UpdatePlayers;

            SyncGameManager.onCountdown -= Countdown;
            SyncGameManager.onPrepareLevel -= PrepareLevel;
            SyncGameManager.onStartLevel -= StartLevel;
            SyncGameManager.onHidePanels -= HidePanels;
        }

        #region PlayerSync

        private void InitializePlayers()
        {
            foreach (var player in GameClient.Instance.Players)
            {
                var playerManager = _initializeGame.InitializePlayer(player);
                playerManager.PlayerStateManager = playerManager.gameObject.AddComponent<PlayerStateManager>();
                _controlSLA.PlayerManagers[player.Id] = playerManager;

                if (player.Id == GameClient.Instance.Id)
                {
                    _initializeGame.InitializeControls(playerManager);
                }
            }
        }

        private void SpawnPlayers(List<PlayerState> playerStates)
        {
            foreach (var playerState in playerStates)
            {
                if (_controlSLA.PlayerManagers.ContainsKey(playerState.Id))
                {
                    _initializeGame.SpawnPlayer(_controlSLA.PlayerManagers[playerState.Id],
                        new Vector3(playerState.PosX, 0, playerState.PosZ), playerState.Rotation);
                }
                else
                {
                    Debug.LogError("No player with ID = " + playerState.Id + " found!");
                }
            }
        }

        private void PlayerDeath(uint playerId)
        {
            if (_controlSLA.PlayerManagers.ContainsKey(playerId))
            {
                _death.Death(_controlSLA.PlayerManagers[playerId]);
            }
            else
            {
                Debug.LogError("No player with ID = " + playerId + " found!");
            }
        }

        private void UpdatePlayers(List<PlayerState> playerStates)
        {
            foreach (var state in playerStates)
            {
                if (_controlSLA.PlayerManagers.ContainsKey(state.Id))
                {
                    _controlSLA.PlayerManagers[state.Id].PlayerStateManager
                        ?.UpdatePosition(state.PosX, state.PosZ, state.Rotation);
                }
                else
                {
                    Debug.LogError("No player with ID = " + state.Id + " found!");
                }
            }
        }

        #endregion

        #region SyncGame

        private void Countdown(ushort counter)
        {
            if (_initializeGame.transform.parent.gameObject.activeSelf)
            {
                _initializeGame.Countdown(counter);
            }
        }

        private void PrepareLevel(int currentLevel)
        {
            _controlSLA.CurrentLevel = currentLevel;
            _initializeGame.PrepareLevel();
        }

        private void StartLevel()
        {
            foreach (var playerManager in _controlSLA.PlayerManagers.Values)
            {
                _initializeGame.StartLevel(playerManager);
            }
        }

        private void HidePanels()
        {
            _initializeGame.HidePanels();
        }

        #endregion
    }
}
