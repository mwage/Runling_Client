using DarkRift;
using Game.Scripts.Drones;
using Game.Scripts.Network.DarkRiftTags;
using Game.Scripts.Network.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Scripts.Synchronization
{
    public class SyncDroneServer : MonoBehaviour
    {
        [SerializeField] private DroneFactory _droneFactory;

        private void Awake()
        {
            _droneFactory.onSpawnDrones += SpawnDrones;
            _droneFactory.onDestroyDrone += DestroyDrone;
        }

        private void OnDestroy()
        {
            _droneFactory.onSpawnDrones -= SpawnDrones;
            _droneFactory.onDestroyDrone -= DestroyDrone;
        }

        #region NetworkCalls

        public void SpawnDrones(List<SpawnDroneData> droneDatas)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                foreach (var droneData in droneDatas)
                {
                    writer.Write(droneData);
                }

                using (var msg = Message.Create(SyncDroneTags.SpawnDrone, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }
        }

        public static void UpdateDroneData(List<DroneState> droneStates)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                foreach (var state in droneStates)
                {
                    writer.Write(state);
                }

                // TODO: Change sendmode to unreliable with jitterbuffer

                using (var msg = Message.Create(SyncDroneTags.UpdateDroneState, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }
        }

        public void DestroyDrone(ushort droneId)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(droneId);

                using (var msg = Message.Create(SyncDroneTags.DestroyDrone, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }
        }

        #endregion

    }
}
