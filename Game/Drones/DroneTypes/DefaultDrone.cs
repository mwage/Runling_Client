using Game.Scripts.Drones.Movement;
using UnityEngine;

namespace Game.Scripts.Drones.DroneTypes
{
    public class DefaultDrone : ADrone
    {
        private readonly Vector3 _position;
        private readonly float _direction;

        public DefaultDrone(IDrone sourceDrone, Vector3 position, float direction, IDroneMovement movementType = null)
        {
            CopyFrom(sourceDrone);
            _position = position;
            _direction = direction;
            if (movementType != null)
            {
                MovementType = movementType;
            }
        }

        public DefaultDrone(float speed, float size, DroneColor color, Vector3? position = null, float direction = 0, DroneType droneType = DroneType.FlyingOneWayDrone, 
            IDroneMovement movementType = null) : base(speed, size, color, droneType, movementType)
        {
            _position = position ?? new Vector3(0, 0.4f, 0);
            _direction = direction;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            return Object.Instantiate(factory.SetDroneType[DroneType], _position, Quaternion.Euler(0, _direction, 0), factory.transform);
        }
    }
}
