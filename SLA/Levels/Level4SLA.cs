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
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(5f, 1f, Color.blue), 10, 4f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(5f, 1.5f, Color.red), 8, 8f, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.StartCoroutine(GreenDronesLevel4(5f, 16, 8f, 1.5f, Color.green, 0.1f, 1f, 1, 16));
        }

        private IEnumerator GreenDronesLevel4(float delay, int initialDroneCount, float speed, float size, Color color, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var droneCount = 0;
            while (true)
            {
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, color, initialDroneCount + 2 * droneCount, false), area: BoundariesSLA.FlyingSla);
                yield return new WaitForSeconds(delay);
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, color, initialDroneCount + 2 * droneCount, true), area: BoundariesSLA.FlyingSla);
                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay*reduceDelay; }
                if (droneCount < maxDrones) { droneCount += droneIncrease; }
            }
        }
    }
}
