using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level12SLA : ALevel
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
            DroneFactory.StartCoroutine(GreenDronesLevel12(4f, 8f, 1.5f));
        }

        private IEnumerator GreenDronesLevel12(float delay, float speed, float size)
        {
            while (true)
            {
                var droneCount = 0;
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.green, 24 + droneCount, true, 0.2f));
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.green, 24 + droneCount, false, 0.2f));
                yield return new WaitForSeconds(delay);
                if (delay > 1.5f) { delay -= delay * 0.03f; }
                if (droneCount < 24) { droneCount += 2; }
            }
        }
    }
}
