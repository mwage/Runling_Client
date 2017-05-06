using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Drones
{
    public class MineVariations
    {
        public static void Timed360Mines(DroneFactory factory, int mineCount, IDrone mineDrone, Area area, IDrone spawnedDrones, 
            int spawnedDroneCount, int pulseDelay, float? reduceDelay = null, float? minDelay = null)
        {
            float minDel = minDelay ?? 1;
            var mines = new List<GameObject>();
            mines.AddRange(factory.SpawnDrones(mineDrone, mineCount, area: area));

            factory.StartCoroutine(Generate360Drones(factory, mines, spawnedDrones, spawnedDroneCount, pulseDelay, minDel, reduceDelay));
        }

        private static IEnumerator Generate360Drones(DroneFactory factory, List<GameObject> mines, IDrone spawnedDrones, int spawnedDroneCount, 
            int pulseDelay, float minDelay, float? reduceDelay = null)
        {
            for (var i = 0; i < mines.Count; i++)
            {
                factory.AddPattern(new Pat360Drones(spawnedDroneCount, repeat: true, pulseDelay: pulseDelay, 
                    reducePulseDelay: reduceDelay, minPulseDelay: mines.Count * minDelay), mines[i], spawnedDrones);
                yield return new WaitForSeconds((float)pulseDelay / mines.Count);
            }
        }
    }
}