using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Drones.DroneTypes;
using Drones.Pattern;

namespace Drones
{
    public class MineVariations
    {
        public static void Synced360Mines(DroneFactory factory, int mineCount, IDrone mineDrone, Area area, IDrone spawnedDrones, 
            int spawnedDroneCount, float delay, float? reduceDelay = null, float? minDelay = null)
        {
            var minDel = minDelay ?? 1;
            var mines = new List<GameObject>();
            mines.AddRange(factory.SpawnDrones(mineDrone, mineCount, area: area));

            factory.StartCoroutine(Generate360Drones(factory, mines, spawnedDrones, spawnedDroneCount, delay, minDel, reduceDelay));
        }

        private static IEnumerator Generate360Drones(DroneFactory factory, ICollection<GameObject> mines, IDrone spawnedDrones, int spawnedDroneCount, 
            float delay, float minDelay, float? reduceDelay = null)
        {
            while (true)
            {
                foreach (var m in mines)
                {
                    factory.AddPattern(new Pat360Drones(spawnedDroneCount), m, spawnedDrones);
                    yield return new WaitForSeconds(delay / mines.Count);
                }
                if (reduceDelay != null) { delay = delay > minDelay * mines.Count ? delay - delay * reduceDelay.Value : 3f; }
            }
        }
    }
}