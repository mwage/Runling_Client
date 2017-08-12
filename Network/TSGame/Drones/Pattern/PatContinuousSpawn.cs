using System.Collections;
using MP.TSGame.Drones.Types;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Pattern
{
    public class PatContinuousSpawn : APattern
    {
        protected readonly float Delay;
        protected readonly int DroneCount;

        public PatContinuousSpawn(float delay, int droneCount)
        {
            Delay = delay;
            DroneCount = droneCount;
        }

        public override void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null)
        {
            if (posDelegate == null)
                posDelegate = delegate { return new TSVector(0, 0.4f, 0); };

            TrueSyncManager.SyncedStartCoroutine(GenerateDrones(factory, drone, posDelegate));
        }

        public override void AddPattern(DroneFactory factory, GameObject drone, IDrone addedDrone, Area area)
        {
            TrueSyncManager.SyncedStartCoroutine(GenerateDrones(factory, addedDrone, delegate { return TSVector.zero; }, drone));
        }

        private IEnumerator GenerateDrones(DroneFactory factory, IDrone drone, StartPositionDelegate posDelegate, GameObject parentDrone = null)
        {
            var levelCounter = factory.LevelCounter;
            var addPattern = parentDrone != null;

            while (true)
            {
                if (parentDrone == null && addPattern)
                    yield break;

                if (levelCounter != factory.LevelCounter)
                    yield break;

                if (parentDrone != null)
                {
                    factory.SpawnDrones(drone, DroneCount, posDelegate: delegate
                    {
                        return parentDrone.GetComponent<TSTransform>().position;
                    });
                }
                else
                {
                    factory.SpawnDrones(drone, DroneCount, posDelegate: posDelegate);
                }
                yield return Delay;
            }
        }
    }
}
