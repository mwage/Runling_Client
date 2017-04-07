using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class RandomFlyingBouncingDrone : ADrone
    {
        public RandomFlyingBouncingDrone(float speed, float size, Color color) : base(speed, size, color)
        {
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area)
        {
            var pos = isAdded
                ? DroneStartPosition.GetRandomCornerAir(Size, area)
                : DroneStartPosition.GetRandomPositionAir(Size, area);

            return Object.Instantiate(factory.FlyingBouncingDrone, pos, Quaternion.Euler(0, DroneDirection.RandomDirection(1f), 0));
        }
    }
}
