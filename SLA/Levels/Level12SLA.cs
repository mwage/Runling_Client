using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level12SLA : ALevelSLA
    {
        public Level12SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn drones (dronecount/delay, speed, size, color)
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1.5f, Color.red), 15, 7f);

            // Spawn green drones (initial delay, size)
            DroneFactory.StartCoroutine(GreenDronesLevel12(4f, 9f, 1.2f, 32, 0.03f, 1.5f, 1, 16));
        }

        private IEnumerator GreenDronesLevel12(float delay, float speed, float size, int initialDroneCount, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            while (true)
            {
                var droneCount = 0;
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.cyan, initialDroneCount + droneCount, true, 0.2f));
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.cyan, initialDroneCount + droneCount, false, 0.2f));
                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones) { droneCount += droneIncrease; }
            }
        }
    }
}
