using Assets.Scripts.SLA.Levels;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class MineDrone : ADrone
    {
        public MineDrone(float speed, float size, Color color) : base(speed, size, color)
        {
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded)
        {
            var direction = Random.Range(0, 4);
            var newDrone = Object.Instantiate(factory.MineDrone, DroneStartPosition.RandomPositionGround(Size, BoundariesSLA.FlyingSla), Quaternion.Euler(0, -45 + 90 * direction, 0));
            return newDrone;
        }
    }
}
