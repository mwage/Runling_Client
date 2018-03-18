using Game.Scripts.Drones;
using Game.Scripts.Network.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Scripts.Synchronization
{
    public class SyncronizationCenter : MonoBehaviour
    {
        [SerializeField] private DroneFactory _droneFactory;

        private const float SendRateDrones = 60;
        private const float MinMoveDistanceDrone = 0.05f;

        private const float SendRatePlayers = 60;
        private const float MinMoveDistancePlayer = 0.05f;


        private IControlServer _control;

        private readonly List<DroneState> _dronesToUpdate = new List<DroneState>();
        private readonly List<PlayerState> _playersToUpdate = new List<PlayerState>();

        private void Awake()
        {
            _control = _droneFactory.transform.parent.GetComponent<IControlServer>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(UpdateDroneState), 0, 1/SendRateDrones);
            InvokeRepeating(nameof(UpdatePlayerState), 0, 1/SendRatePlayers);
        }

        private void UpdateDroneState()
        {
            foreach (var drone in _droneFactory.Drones.Values)
            {
                var positionData = drone.GetPositionData(MinMoveDistanceDrone);
                if (positionData != null)
                {
                    _dronesToUpdate.Add(positionData);
                }
            }

            if (_dronesToUpdate.Count == 0)
                return;

            SyncDroneServer.UpdateDroneData(_dronesToUpdate);
            _dronesToUpdate.Clear();
        }

        private void UpdatePlayerState()
        {
            foreach (var playerManager in _control.PlayerManagers.Values)
            {
                var positionData = playerManager.PlayerStateManager.GetPositionData(MinMoveDistancePlayer);
                if (positionData != null)
                {
                    _playersToUpdate.Add(positionData);
                }
            }

            if (_playersToUpdate.Count == 0)
                return;

            SyncPlayerServer.UpdatePlayerData(_playersToUpdate);
            _playersToUpdate.Clear();
        }
    }
}
