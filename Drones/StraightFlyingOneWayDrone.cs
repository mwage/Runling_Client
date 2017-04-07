using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Drones
{
    public class StraightFlyingOnewayDrone : ADrone
    {
        protected readonly Vector3 Position;
        protected readonly float Direction;

        public StraightFlyingOnewayDrone(float speed, float size, Color color, Vector3 position, float direction) : base(speed, size, color)
        {
            Position = position;
            Direction = direction;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area)
        {
            return Object.Instantiate(factory.FlyingOnewayDrone, Position, Quaternion.Euler(0, Direction, 0));
        }
    }
}
