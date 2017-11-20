using DarkRift;
using DarkRift.Client;
using Drones;
using Drones.DroneTypes;
using Network.DarkRiftTags;
using Network.Synchronization.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Network.Synchronization
{
    public class SyncDroneManager : MonoBehaviour
    {
        [SerializeField] private DroneFactory _droneFactory;
        
        #region Events

        public delegate void SpawnDroneEventHandler(IDrone drone);
        public delegate void UpdateDronesEventHandler(List<DroneState> drones);
        public delegate void DestroyDroneEventHandler(ushort id);

        public static event SpawnDroneEventHandler onSpawnDrone;
        public static event UpdateDronesEventHandler onUpdateDrones;
        public static event DestroyDroneEventHandler onDestroyDrone;
        
        #endregion

        private void Awake()
        {
            GameClient.Instance.MessageReceived += OnDataHandler;
        }

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.SyncDrone)
                return;

            if (message.Subject == SyncDroneSubjects.SpawnDrone)
            {
                var reader = message.GetReader();
                var droneDatas = new List<SpawnDroneData>();

                while (reader.Position < reader.Length)
                {
                    droneDatas.Add(reader.ReadSerializable<SpawnDroneData>());
                }

                foreach (var data in droneDatas)
                {
                    var drone = new NetworkedDrone(data.Speed, data.Size, data.Color, data.DroneType, data.State);
                    _droneFactory.SpawnDrones(drone);
                }
            }
            else if (message.Subject == SyncDroneSubjects.UpdateDroneState)
            {
                var droneStates = new List<DroneState>();
                var reader = message.GetReader();
                while (reader.Position < reader.Length)
                {
                    droneStates.Add(reader.ReadSerializable<DroneState>());
                }

                foreach (var drone in droneStates)
                {
                    if (_droneFactory.Drones.ContainsKey(drone.Id))
                    {
                        _droneFactory.Drones[drone.Id].UpdatePosition(drone.PosX, drone.PosZ);
                    }
                    else
                    {
                        Debug.LogError("No drone with ID = " + drone.Id + " found!");
                    }
                }
            }
            else if (message.Subject == SyncDroneSubjects.DestroyDrone)
            {
                var reader = message.GetReader();
                var droneId = reader.ReadUInt16();

                if (_droneFactory.Drones.ContainsKey(droneId))
                {
                    Destroy(_droneFactory.Drones[droneId].gameObject);
                    _droneFactory.Drones.Remove(droneId);
                }
                else
                {
                    Debug.LogError("No drone with ID = " + droneId + " found!");
                }
            }
        }
    }
}
