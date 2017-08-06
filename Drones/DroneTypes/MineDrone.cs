using System.IO;
using Drones.Movement;
using Drones.Pattern;
using UnityEngine;

namespace Drones.DroneTypes
{
    public class MineDrone : ADrone
    {
        protected readonly IPattern Pattern;
        protected readonly IDrone SpawnedDrones;
                
        public MineDrone(float speed, float size, DroneColor color, IPattern pattern = null, IDrone spawnedDrones = null, DroneType? droneType = null, 
            DroneMovement.MovementDelegate moveDelegate = null, float? curving = null, float? sinForce = null, float? sinFrequency = null, GameObject chaserTarget = null) : 
            base(speed, size, color, droneType, moveDelegate, curving, sinForce, sinFrequency, chaserTarget)
        {
            DroneType = droneType ?? DroneType.FlyingBouncingMine;
            Pattern = pattern;
            SpawnedDrones = spawnedDrones;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var direction = Random.Range(0, 4);
            var newDrone = Object.Instantiate(factory.SetDroneType[DroneType], DroneStartPosition.GetRandomPosition(Size, area), 
                Quaternion.Euler(0, -45 + 90 * direction, 0), factory.transform);

            if (Pattern != null)
            {
                factory.AddPattern(Pattern, newDrone, SpawnedDrones);
            }
            return newDrone;
        }
    }
}
