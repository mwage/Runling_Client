using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Drones
{
    public class MineVariations
    {
        public static void Add360Drones(int droneCount, float delay, float speed, float size, Color color, List<GameObject> mines, DroneFactory factory, float? reduceDelay = null, DroneMovement.MovementDelegate moveDelegate = null)
        {
            factory.StartCoroutine(Generate360Drones(droneCount, delay, speed, size, color, mines, factory, reduceDelay, moveDelegate));
        }

        public static void AddDelayed360Drones(int droneCount, float delay, int rotations, float speed, float size, Color color, GameObject[] mines, DroneFactory factory, DroneMovement.MovementDelegate moveDelegate = null)
        {
            foreach (var mine in mines)
            {
                factory.StartCoroutine(GenerateDelayedStraightFlying360Drones(droneCount, delay, rotations, speed, size,
                    color, mine, factory, moveDelegate));
            }
        }

        private static IEnumerator Generate360Drones(int droneCount, float delay, float speed, float size, Color color, List<GameObject> mines, DroneFactory factory, float? reduceDelay = null, DroneMovement.MovementDelegate moveDelegate = null)
        {
            while (true)
            {
                foreach (var m in mines)
                {
                    factory.SetPattern(new Pat360Drones(droneCount),
                        new OnewayDrone(speed, size, color), posDelegate: delegate { return m.transform.position; }, moveDelegate: moveDelegate);
                    yield return new WaitForSeconds(delay / mines.Count);
                }

                if (reduceDelay != null) { delay = delay > mines.Count ? delay - delay * reduceDelay.Value : 3f; }
            }
        }

        private static IEnumerator GenerateDelayedStraightFlying360Drones(int droneCount, float delay, int rotations, float speed, float size, Color color, GameObject mine, DroneFactory factory, DroneMovement.MovementDelegate moveDelegate = null)
        {
            while (true)
            {
                for (var j = 0; j < rotations; j++)
                {
                    for (var i = 0; i < droneCount; i++)
                    {
                        factory.SpawnDrones(new OnewayDrone(speed, size, color, mine.transform.position, i * 360f / droneCount), moveDelegate: moveDelegate);
                        yield return new WaitForSeconds(delay / droneCount);
                    }
                }
                for (var j = 0; j < rotations; j++)
                {
                    for (var i = 0; i < droneCount; i++)
                    {
                        factory.SpawnDrones(new OnewayDrone(speed, size, color, mine.transform.position, -i * 360f / droneCount), moveDelegate: moveDelegate);
                        yield return new WaitForSeconds(delay / droneCount);
                    }
                }
            }
        }
    }
}
