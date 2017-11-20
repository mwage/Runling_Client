using DarkRift;
using Drones;
using Network.Synchronization.Data;
using Server.Scripts.Synchronization;
using UnityEngine;

namespace Network.Synchronization
{
    public class DroneStateManager : MonoBehaviour
    {
        public ushort Id { get; set; }
        public DroneFactory DroneFactory { get; set; }

        private Vector3 _lastSentPosition;

        private void Awake()
        {
            _lastSentPosition = transform.position;
        }

        private void OnDestroy()
        {
            if (DroneFactory == null)
                return;

            if (DroneFactory.IsServer)
            {
                SyncDroneServer.DestroyDrone(Id);
            }

            DroneFactory.Drones.Remove(Id);
        }

        public DroneState GetPositionData(float minMoveDistance)
        {
            if (Vector3.Distance(transform.position, _lastSentPosition) > minMoveDistance)
            {
                _lastSentPosition = transform.position;
                return new DroneState(Id, _lastSentPosition.x, _lastSentPosition.z);
            }
            return null;
        }

        public void UpdatePosition(float posX, float posZ)
        {
            // TODO: Lerp
            transform.position = new Vector3(posX, transform.position.y, posZ);
        }
    }
}
