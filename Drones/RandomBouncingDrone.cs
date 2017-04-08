using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class RandomBouncingDrone : ADrone
    {
        public RandomBouncingDrone(float speed, float size, Color color) : base(speed, size, color)
        {
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            Vector3 pos;
            if (posDelegate != null)
                pos = posDelegate(Size, area);
            else
            {
                pos = isAdded
                ? DroneStartPosition.GetRandomCornerGround(Size, area)
                : DroneStartPosition.GetRandomPositionGround(Size, area);
            }

            var newDrone = Object.Instantiate(factory.BouncingDrone, pos, Quaternion.Euler(0, DroneDirection.RandomDirection(1f), 0));
            return newDrone;
        }
    }
}
