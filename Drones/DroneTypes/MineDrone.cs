using Drones.Movement;
using Drones.Pattern;
using UnityEngine;

namespace Drones.DroneTypes
{
    public class MineDrone : ADrone
    {
        public MineDrone(float speed, float size, DroneColor color, IPattern pattern = null, IDrone spawnedDrones = null, DroneType droneType = DroneType.FlyingBouncingMine,
            IDroneMovement movementType = null) : base(speed, size, color, droneType, movementType, pattern, spawnedDrones)
        {
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var direction = Random.Range(0, 4);
            var newDrone = Object.Instantiate(factory.SetDroneType[DroneType], DroneStartPosition.GetRandomPosition(Size, area), 
                Quaternion.Euler(0, -45 + 90 * direction, 0), factory.transform);

            return newDrone;
        }
    }
}
