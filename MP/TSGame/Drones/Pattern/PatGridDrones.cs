using System.Collections;
using MP.TSGame.Drones.Types;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Pattern
{
    public class PatGridDrones : APattern
    {
        protected int DroneCount;
        protected float Delay;
        protected bool AddDrones;

        public PatGridDrones(int droneCount, float delay, bool? addDrones = null)
        {
            DroneCount = droneCount;
            Delay = delay;
            AddDrones = addDrones ?? false;
        }

        public override void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null)
        {
            TrueSyncManager.SyncedStartCoroutine(GenerateHorizontalGridDrones(drone, DroneCount, Delay, area, factory, AddDrones));
            TrueSyncManager.SyncedStartCoroutine(GenerateVerticalGridDrones(drone, DroneCount, Delay, area, factory, AddDrones));
        }
        
        private static IEnumerator GenerateHorizontalGridDrones(IDrone drone, int droneCount, float delay, Area area, DroneFactory factory, bool addDrones)
        {
            var levelCounter = factory.LevelCounter;
            var height = area.TopBoundary - (0.5f + (float)drone.Size / 2);
            var length = area.RightBoundary - (0.5f + (float)drone.Size / 2);
            const float direction = 90f;

            while (true)
            {
                for (var j = 0; j < (int)(length / height); j++)
                {
                    for (var i = 0; i < droneCount; i++)
                    {
                        if (levelCounter != factory.LevelCounter)
                            yield break;

                        var startPos = new Vector3(-length, 0.4f, height - i * 2 * height / droneCount);
                        factory.SpawnDrones(new DefaultDrone(drone, new TSVector(startPos.x, startPos.y, startPos.z), direction));

                        yield return delay * 2 * height / droneCount;
                    }
                    for (var i = 0; i < droneCount; i++)
                    {
                        if (levelCounter != factory.LevelCounter)
                            yield break;

                        var startPos = new Vector3(-length, 0.4f, -height + i * 2 * height / droneCount);
                        factory.SpawnDrones(new DefaultDrone(drone, new TSVector(startPos.x, startPos.y, startPos.z), direction));

                        yield return delay * 2 * height / droneCount;
                    }
                }

                if (addDrones)
                    droneCount++;
            }
        }

        private static IEnumerator GenerateVerticalGridDrones(IDrone drone, int droneCount, float delay, Area area, DroneFactory factory, bool addDrones)
        {
            var levelCounter = factory.LevelCounter;
            var height = area.TopBoundary - (0.5f + (float)drone.Size / 2);
            var lenght = area.RightBoundary - (0.5f + (float)drone.Size / 2);
            const float direction = 180f;
            droneCount *= (int)(lenght / height);

            while (true)
            {
                for (var i = 0; i < droneCount; i++)
                {
                    if (levelCounter != factory.LevelCounter)
                        yield break;

                    var startPos = new Vector3(-lenght + i * 2 * lenght / droneCount, 0.4f, height);
                    factory.SpawnDrones(new DefaultDrone(drone, new TSVector(startPos.x, startPos.y, startPos.z), direction));

                    yield return delay * 2 * lenght / droneCount;
                }
                for (var i = 0; i < droneCount; i++)
                {
                    if (levelCounter != factory.LevelCounter)
                        yield break;

                    var startPos = new Vector3(lenght - i * 2 * lenght / droneCount, 0.4f, height);
                    factory.SpawnDrones(new DefaultDrone(drone, new TSVector(startPos.x, startPos.y, startPos.z), direction));

                    yield return delay * 2 * lenght / droneCount;
                }

                if (addDrones)
                    droneCount += (int)(lenght / height);
            }
        }
    }
}
