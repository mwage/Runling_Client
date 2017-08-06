using Drones.Movement;
using Players;
using UnityEngine;

namespace Drones.DroneTypes
{
    public class DefaultDrone : ADrone
    {
        protected readonly Vector3 Position;
        protected readonly float Direction;

        public DefaultDrone(IDrone sourceDrone, Vector3 position, float direction, PlayerManager chaserTarget = null)
        {
            CopyFrom(sourceDrone);
            Position = position;
            Direction = direction;
            ChaserTarget = chaserTarget;
        }

        public DefaultDrone(float speed, float size, DroneColor color, Vector3? position = null, float? direction = null, DroneType? droneType = null, 
            DroneMovement.MovementDelegate moveDelegate = null, float? curving = null, float? sinForce = null, float? sinFrequency = null, PlayerManager chaserTarget = null) : 
            base(speed, size, color, droneType, moveDelegate, curving, sinForce, sinFrequency, chaserTarget)
        {
            Position = position ?? new Vector3(0, 0.4f, 0);
            Direction = direction ?? 0;
            DroneType = droneType ?? DroneType.FlyingOneWayDrone;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            return Object.Instantiate(factory.SetDroneType[DroneType], Position, Quaternion.Euler(0, Direction, 0), factory.transform);
        }
    }
}
