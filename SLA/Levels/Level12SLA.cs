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
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1.5f, Color.red), 15, 7f, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.StartCoroutine(GenerateLevel12GreenDrones(4f, 9f, 1.2f, Color.cyan, 32, 0.03f, 1.5f, 1, 16));
        }

        private IEnumerator GenerateLevel12GreenDrones(float delay, float speed, float size, Color color, int initialDroneCount, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var droneCount = 0;

            while (true)
            {
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, 5f),
                    new OnewayDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomBottomSector);
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, 5f),
                    new OnewayDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomTopSector);

                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones) { droneCount += droneIncrease; }
            }
        }
    }
}
