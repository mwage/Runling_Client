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


        private IEnumerator IStraightFlying360Drones(int droneCount, float delay, float speed, float size, Color color, GameObject[] mines, DroneFactory DroneFactory, float? reduceDelay = null)
        {
            while (true)
            {
                for (var i = 0; i < mines.Length; i++)
                {
                    DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, color, droneCount, false, position: mines[i].transform.position));
                    yield return new WaitForSeconds(delay / mines.Length);
                }

                if (reduceDelay != null) { delay = delay > 3f ? delay -= delay * reduceDelay.Value : 3f; }
            }
        }
    }
}
