using DarkRift.Client;
using Game.Scripts.Drones;
using Game.Scripts.Drones.DroneTypes;
using Game.Scripts.Network.DarkRiftTags;
using Game.Scripts.Network.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Network.Syncronization
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
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.SyncDrone || message.Tag >= Tags.TagsPerPlugin * (Tags.SyncDrone + 1))
                    return;

                if (message.Tag == SyncDroneTags.UpdateDroneState)
                {
                    var droneStates = new List<DroneState>();
                    using (var reader = message.GetReader())
                    {
                        while (reader.Position < reader.Length)
                        {
                            droneStates.Add(reader.ReadSerializable<DroneState>());
                        }
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
                else if (message.Tag == SyncDroneTags.SpawnDrone)
                {
                    var droneDatas = new List<SpawnDroneData>();
                    using (var reader = message.GetReader())
                    {
                        while (reader.Position < reader.Length)
                        {
                            droneDatas.Add(reader.ReadSerializable<SpawnDroneData>());

                        }
                    }
                    foreach (var data in droneDatas)
                    {
                        var drone = new NetworkedDrone(data.Speed, data.Size, data.Color, data.DroneType, data.State);
                        _droneFactory.SpawnDrones(drone);
                    }
                }
                else if (message.Tag == SyncDroneTags.DestroyDrone)
                {
                    ushort droneId;
                    using (var reader = message.GetReader())
                    {
                        droneId = reader.ReadUInt16();
                    }

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
}
