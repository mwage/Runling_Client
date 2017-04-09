using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Drones
{
    public class MineVariations
    {
        public static void AddStraightFlying360Drones(int droneCount, float delay, float speed, float size, Color color, GameObject[] mines, DroneFactory factory, float? reduceDelay = null)
        {
            factory.StartCoroutine(GenerateStraightFlying360Drones(droneCount, delay, speed, size, color, mines, factory, reduceDelay));
        }

        public static void AddDelayedStraightFlying360Drones(int droneCount, float delay, int rotations, float speed, float size, Color color, GameObject mine, DroneFactory factory)
        {
            factory.StartCoroutine(GenerateDelayedStraightFlying360Drones(droneCount, delay, rotations, speed, size, color, mine, factory));
        }

        private static IEnumerator GenerateStraightFlying360Drones(int droneCount, float delay, float speed, float size, Color color, ICollection<GameObject> mines, DroneFactory factory, float? reduceDelay = null)
        {
            while (true)
            {
                foreach (var m in mines)
                {
                    factory.SpawnDrones(new StraightFlying360Drone(speed, size, color, droneCount), posDelegate: delegate { return m.transform.position; });
                    yield return new WaitForSeconds(delay / mines.Count);
                }

                if (reduceDelay != null) { delay = delay > mines.Count ? delay - delay * reduceDelay.Value : 3f; }
            }
        }

        private static IEnumerator GenerateDelayedStraightFlying360Drones(int droneCount, float delay, int rotations, float speed, float size, Color color, GameObject mine, DroneFactory factory)
        {
            while (true)
            {
                for (var j = 0; j < rotations; j++)
                {
                    for (var i = 0; i < droneCount; i++)
                    {
                        factory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, mine.transform.position, i * 360f / droneCount));
                        yield return new WaitForSeconds(delay / droneCount);
                    }
                }
                for (var j = 0; j < rotations; j++)
                {
                    for (var i = 0; i < droneCount; i++)
                    {
                        factory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, mine.transform.position, -i * 360f / droneCount));
                        yield return new WaitForSeconds(delay / droneCount);
                    }
                }
            }
        }
    }
}
