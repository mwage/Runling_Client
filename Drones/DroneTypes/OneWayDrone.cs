using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Drones
{
    public class OnewayDrone : ADrone
    {
        protected readonly Vector3 Position;
        protected readonly float Direction;

        public OnewayDrone(float speed, float size, Color color, Vector3? position = null, float? direction = null, DroneType? droneType = null) : base(speed, size, color, droneType)
        {
            Position = position ?? new Vector3(0, 0.6f, 0);
            Direction = direction ?? 0;
            DroneType = droneType ?? DroneType.FlyingOnewayDrone;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            return Object.Instantiate(DroneFactory.SetDroneType[DroneType], Position, Quaternion.Euler(0, Direction, 0));
        }
    }
}
