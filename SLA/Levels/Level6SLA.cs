using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level6SLA : ALevelSLA
    {
        public Level6SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1f, Color.blue), 15, 6f);
            
            // Spawn green drones (initial delay, speed, size)
            DroneFactory.StartCoroutine(GreenDronesLevel6Bottom(4f, 24, 7f, 1.5f, 0.03f, 1.5f, 2, 24));
            DroneFactory.StartCoroutine(GreenDronesLevel6Top(5f, 24, 7f, 1.5f, 0.04f, 1.5f, 2, 24));
        }

        private IEnumerator GreenDronesLevel6Bottom(float delay, int initialDroneCount, float speed, float size, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            while (true)
            {
                var droneCount = 0;
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.green, initialDroneCount + droneCount, true, 3f / (16 + droneCount)));
                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones) { droneCount += droneIncrease; }
            }
        }

        private IEnumerator GreenDronesLevel6Top(float delay, int initialDroneCount, float speed, float size, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            while (true)
            {
                var droneCount = 0;
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.green, initialDroneCount + droneCount, false, 3f / (16 + droneCount)));
                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones) { droneCount += droneIncrease; }
            }
        }
    }
}
