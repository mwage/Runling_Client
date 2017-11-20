using Network.Synchronization;
using Network.Synchronization.Data;
using UnityEngine;

namespace Drones.DroneTypes
{
    public class NetworkedDrone : ADrone
    {
        private readonly Vector3 _position;
        private readonly DroneState _state;

        public NetworkedDrone(float speed, float size, DroneColor color, DroneType droneType, DroneState state) 
            : base(speed, size, color, droneType, null)
        {
            _position = new Vector3(state.PosX, 0.4f, state.PosZ);
            _state = state;
            MovementType = null;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {

            var drone = Object.Instantiate(factory.SetDroneType[DroneType], _position, Quaternion.identity, factory.transform);
            var data = drone.AddComponent<DroneStateManager>();
            data.Id = _state.Id;
            data.DroneFactory = factory;
            if (factory.Drones.ContainsKey(_state.Id))
            {
                Debug.LogError("ID " + _state.Id + " already taken!");
            }
            factory.Drones[_state.Id] = data;
            return drone;
        }
    }
}
