using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level4SLA : ALevelSLA
    {
        public Level4SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(5f, 1f, Color.blue), 10, 4f);
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(5f, 1.5f, Color.red), 8, 8f);

            // Spawn green drones (initial delay, size)
            DroneFactory.StartCoroutine(GreenDronesLevel4(5f, 16, 8f, 1.5f, 0.1f, 1f, 1, 16));
        }

        private IEnumerator GreenDronesLevel4(float delay, int initialDroneCount, float speed, float size, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var droneCount = 0;
            while (true)
            {
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.green, initialDroneCount + 2 * droneCount, true));
                yield return new WaitForSeconds(delay);
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.green, initialDroneCount + 2 * droneCount, false));
                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay*reduceDelay; }
                if (droneCount < maxDrones) { droneCount += droneIncrease; }
            }
        }
    }
}
