using System;
using System.Collections.Generic;
using Network.Synchronization;
using Network.Synchronization.Data;
using TMPro;
using UnityEngine;

namespace SLA.Network
{
    public class SynchronizationSLA : MonoBehaviour
    {
        [SerializeField] private InitializeGameSLA _initializeGame;

        private ControlSLA _controlSLA;
        private DeathSLA _death;

        private void Awake()
        {
            _death = _initializeGame.gameObject.GetComponent<DeathSLA>();
            _controlSLA = _initializeGame.gameObject.GetComponent<ControlSLA>();

            SyncPlayerManager.onSpawnPlayers += SpawnPlayer;
            SyncPlayerManager.onPlayerDeath += PlayerDeath;
            SyncPlayerManager.onUpdatePlayers += UpdatePlayers;

            SyncGameManager.onCountdown += Countdown;
        }

        private void OnDestroy()
        {
            SyncPlayerManager.onSpawnPlayers -= SpawnPlayer;
            SyncPlayerManager.onPlayerDeath -= PlayerDeath;
            SyncPlayerManager.onUpdatePlayers -= UpdatePlayers;
        }

        #region PlayerSync

        private void SpawnPlayer(PlayerState playerState)
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
                        .UpdatePosition(state.PosX, state.PosZ, state.Rotation);
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

        #endregion
    }
}
