using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class MineDrone : ADrone
    {
        protected readonly IPattern Pattern;
        protected readonly IDrone SpawnedDrones;
        public MineDrone(float speed, float size, Color color, IPattern pattern, IDrone spawnedDrones, DroneType? droneType = null) : base(speed, size, color, droneType)
        {
            DroneType = droneType ?? DroneType.MineDrone;
            Pattern = pattern;
            SpawnedDrones = spawnedDrones;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var direction = Random.Range(0, 4);
            var newDrone = Object.Instantiate(DroneFactory.GetDroneType[DroneType], DroneStartPosition.GetRandomPositionGround(Size, area), Quaternion.Euler(0, -45 + 90 * direction, 0));
            factory.AddPattern(Pattern, newDrone, SpawnedDrones);
            return newDrone;
        }
    }
}
