using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Drones
{
    public class DefaultDrone : ADrone
    {
        protected readonly Vector3 Position;
        protected readonly float Direction;

        public DefaultDrone(IDrone sourceDrone, Vector3 position, float direction)
        {
            CopyFrom(sourceDrone);
            Position = position;
            Direction = direction;
        }

        public DefaultDrone(float speed, float size, DroneColor color, Vector3? position = null, float? direction = null, DroneType? droneType = null, 
            DroneMovement.MovementDelegate moveDelegate = null, float? curving = null, float? sinForce = null, float? sinFrequency = null) : 
            base(speed, size, color, droneType, moveDelegate, curving, sinForce, sinFrequency)
        {
            Position = position ?? new Vector3(0, 0.6f, 0);
            Direction = direction ?? 0;
            DroneType = droneType ?? DroneType.FlyingOnewayDrone;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            return Object.Instantiate(factory.SetDroneType[DroneType], Position, Quaternion.Euler(0, Direction, 0));
        }
    }
}
