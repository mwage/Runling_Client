using Assets.Scripts.SLA.Levels;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class RandomFlyingBouncingDrone : ADrone
    {
        public RandomFlyingBouncingDrone(float speed, float size, Color color) : base(speed, size, color)
        {
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded)
        {
            var pos = isAdded
                ? DroneStartPosition.RandomCornerAir(Size, BoundariesSLA.FlyingSla)
                : DroneStartPosition.RandomPositionAir(Size, BoundariesSLA.FlyingSla);

            return Object.Instantiate(factory.FlyingBouncingDrone, pos, Quaternion.Euler(0, DroneDirection.RandomDirection(1f), 0));
        }
    }
}
