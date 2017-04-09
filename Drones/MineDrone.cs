using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class MineDrone : ADrone
    {
        public MineDrone(float speed, float size, Color color) : base(speed, size, color)
        {
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var direction = Random.Range(0, 4);
            var newDrone = Object.Instantiate(factory.MineDrone, DroneStartPosition.GetRandomPositionGround(Size, area), Quaternion.Euler(0, -45 + 90 * direction, 0));
            return newDrone;
        }
    }
}
