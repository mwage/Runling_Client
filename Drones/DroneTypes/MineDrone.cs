using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class MineDrone : ADrone
    {
        protected readonly IPattern Pattern;
        protected readonly IDrone SpawnedDrones;
        protected readonly DroneMovement.MovementDelegate SpawnedDronesMovement;
        
        public MineDrone(float speed, float size, Color color, IPattern pattern = null, IDrone spawnedDrones = null, DroneType? droneType = null, DroneMovement.MovementDelegate spawnedDronesMovement = null) : base(speed, size, color, droneType)
        {
            DroneType = droneType ?? DroneType.MineDrone;
            Pattern = pattern;
            SpawnedDrones = spawnedDrones;
            SpawnedDronesMovement = spawnedDronesMovement;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var direction = Random.Range(0, 4);
            var newDrone = Object.Instantiate(DroneFactory.SetDroneType[DroneType], DroneStartPosition.GetRandomPositionGround(Size, area), Quaternion.Euler(0, -45 + 90 * direction, 0));
            if (Pattern != null)
            {
                factory.AddPattern(Pattern, newDrone, SpawnedDrones, moveDelegate: SpawnedDronesMovement);
            }
            return newDrone;
        }
    }
}
