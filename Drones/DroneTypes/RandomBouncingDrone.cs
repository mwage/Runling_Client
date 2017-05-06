using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class RandomBouncingDrone : ADrone
    {
        public RandomBouncingDrone(float speed, float size, Color color, DroneType? droneType = null) : base(speed, size, color, droneType)
        {
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var newDrone = Object.Instantiate(DroneFactory.GetDroneType[DroneType]);

            var isGround = newDrone.layer == 11;

            Vector3 pos;
            if (posDelegate != null)
                pos = posDelegate(Size, area);
            else
            {
                if (isGround)
                {
                    pos = isAdded
                        ? DroneStartPosition.GetRandomCornerGround(Size, area)
                        : DroneStartPosition.GetRandomPositionGround(Size, area);
                }
                else
                {
                    pos = isAdded
                        ? DroneStartPosition.GetRandomCornerAir(Size, area)
                        : DroneStartPosition.GetRandomPositionAir(Size, area);
                }
            }

            newDrone.transform.position = pos;
            newDrone.transform.rotation = Quaternion.Euler(0, DroneDirection.RandomDirection(1f), 0);

            return newDrone;
        }
    }
}
