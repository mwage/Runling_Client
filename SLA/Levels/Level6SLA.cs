using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level6SLA : ALevel
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
            DroneFactory.StartCoroutine(GreenDronesLevel6Bottom(4f, 7f, 1.5f));
            DroneFactory.StartCoroutine(GreenDronesLevel6Top(5f, 7f, 1.5f));
        }

        private IEnumerator GreenDronesLevel6Bottom(float delay, float speed, float size)
        {
            while (true)
            {
                var droneCount = 0;
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.green, 24+droneCount, true, 3f / (16 + droneCount)));
                yield return new WaitForSeconds(delay);
                if (delay > 1.5f) { delay -= delay * 0.03f; }
                if (droneCount < 24) { droneCount +=2; }
            }
        }

        private IEnumerator GreenDronesLevel6Top(float delay, float speed, float size)
        {
            while (true)
            {
                var droneCount = 0;
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.green, 24 + droneCount, false, 3f / (16 + droneCount)));
                yield return new WaitForSeconds(delay);
                if (delay > 1.5f) { delay -= delay * 0.04f; }
                if (droneCount < 24) { droneCount +=2; }
            }
        }
    }
}
