using System.Collections;
using UnityEngine;


namespace Assets.Scripts.Drones
{
    public class MineVariations
    {
        public void StraightFlying360Drones(int droneCount, float delay, float speed, float size, Color color, GameObject[] mines, DroneFactory DroneFactory, float? reduceDelay = null)
        {
            DroneFactory.StartCoroutine(IStraightFlying360Drones(droneCount, delay, speed, size, color, mines, DroneFactory, reduceDelay));
        }

        public void DelayedStraightFlying360Drones(int droneCount, float delay, int rotations, float speed, float size, Color color, GameObject mine, DroneFactory DroneFactory)
        {
            DroneFactory.StartCoroutine(IDelayedStraightFlying360Drones(droneCount, delay, rotations, speed, size, color, mine, DroneFactory));
        }


        private IEnumerator IStraightFlying360Drones(int droneCount, float delay, float speed, float size, Color color, GameObject[] mines, DroneFactory DroneFactory, float? reduceDelay = null)
        {
            while (true)
            {
                for (var i = 0; i < mines.Length; i++)
                {
                    DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, color, droneCount, false, position: mines[i].transform.position));
                    yield return new WaitForSeconds(delay / mines.Length);
                }

                if (reduceDelay != null) { delay = delay > mines.Length ? delay - delay * reduceDelay.Value : 3f; }
            }
        }

        private IEnumerator IDelayedStraightFlying360Drones(int droneCount, float delay, int rotations, float speed, float size, Color color, GameObject mine, DroneFactory DroneFactory)
        {
            while (true)
            {
                for (var j = 0; j < rotations; j++)
                {
                    for (var i = 0; i < droneCount; i++)
                    {
                        DroneFactory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, mine.transform.position, i * 360 / droneCount));
                        yield return new WaitForSeconds(delay / droneCount);
                    }
                }
                for (var j = 0; j < rotations; j++)
                {
                    for (var i = 0; i < droneCount; i++)
                    {
                        DroneFactory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, mine.transform.position, -i * 360 / droneCount));
                        yield return new WaitForSeconds(delay / droneCount);
                    }
                }
            }
        }
    }
}
