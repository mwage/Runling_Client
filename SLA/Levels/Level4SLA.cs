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
            DroneFactory.StartCoroutine(GreenDronesLevel4(5f, 1.5f));
        }

        private IEnumerator GreenDronesLevel4(float delay, float size)
        {
            var droneCount = 0;
            while (true)
            {
                DroneFactory.SpawnDrones(new StraightFlying360Drone(8f, size, Color.green, 16 + 2 * droneCount, true));
                yield return new WaitForSeconds(delay);
                DroneFactory.SpawnDrones(new StraightFlying360Drone(8f, size, Color.green, 16 + 2 * droneCount, false));
                yield return new WaitForSeconds(delay);
                if (delay > 1f) { delay -= delay*0.1f; }
                if (droneCount < 15) { droneCount++; }
            }
        }
    }
}
