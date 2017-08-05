using System.Collections;
using System.Collections.Generic;
using MP.TSGame.Drones.Pattern;
using MP.TSGame.Drones.Types;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones
{
    public class MineVariations
    {
        public static void Synced360Mines(DroneFactory factory, int mineCount, IDrone mineDrone, Area area, IDrone spawnedDrones, 
            int spawnedDroneCount, float delay, float? reduceDelay = null, float? minDelay = null)
        {
            var minDel = minDelay ?? 1;
            var mines = new List<GameObject>();
            mines.AddRange(factory.SpawnDrones(mineDrone, mineCount, area: area));

            TrueSyncManager.SyncedStartCoroutine(Generate360Drones(factory, mines, spawnedDrones, spawnedDroneCount, delay, minDel, reduceDelay));
        }

        private static IEnumerator Generate360Drones(DroneFactory factory, ICollection<GameObject> mines, IDrone spawnedDrones, int spawnedDroneCount, 
            float delay, float minDelay, float? reduceDelay = null)
        {
            var levelCounter = factory.LevelCounter;

            while (true)
            {
                foreach (var m in mines)
                {
                    yield return delay / mines.Count;

                    if (levelCounter != factory.LevelCounter)
                        yield break;

                    factory.AddPattern(new Pat360Drones(spawnedDroneCount, 1), m, spawnedDrones);
                }

                if (reduceDelay != null) { delay = delay > minDelay * mines.Count ? delay - delay * reduceDelay.Value : 3f; }
            }
        }
    }
}