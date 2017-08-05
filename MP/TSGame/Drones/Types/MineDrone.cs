using MP.TSGame.Drones.Movement;
using MP.TSGame.Drones.Pattern;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Types
{
    public class MineDrone : ADrone
    {
        protected readonly IPattern Pattern;
        protected readonly IDrone SpawnedDrones;
                
        public MineDrone(float speed, float size, DroneColor color, IPattern pattern = null, IDrone spawnedDrones = null, DroneType? droneType = null, 
            DroneMovement.MovementDelegate moveDelegate = null, float? curving = null, float? sinForce = null, float? sinFrequency = null) : 
            base(speed, size, color, droneType, moveDelegate, curving, sinForce, sinFrequency)
        {
            DroneType = droneType ?? DroneType.FlyingBouncingMine;
            Pattern = pattern;
            SpawnedDrones = spawnedDrones;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var direction = Random.Range(0, 4);
            var newDrone = TrueSyncManager.SyncedInstantiate(factory.SetDroneType[DroneType], 
                DroneStartPosition.GetRandomPosition((float)Size, area), TSQuaternion.Euler(0, -45 + 90 * direction, 0));
            if (Pattern != null)
            {
                factory.AddPattern(Pattern, newDrone, SpawnedDrones);
            }
            newDrone.transform.SetParent(factory.transform);
            return newDrone;
        }
    }
}
