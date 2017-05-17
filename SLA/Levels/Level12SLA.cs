using System.Collections;
using Drones;
using Drones.DroneTypes;
using Drones.Pattern;
using UnityEngine;

namespace SLA.Levels
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
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7f, 1.5f, DroneColor.Red), 15, 7f, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.StartCoroutine(GenerateLevel12GreenDrones(4f, 9f, 1.2f, DroneColor.Cyan, 32, 0.03f, 1.5f, 1, 48));
        }

        private IEnumerator GenerateLevel12GreenDrones(float delay, float speed, float size, DroneColor color, int initialDroneCount, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var droneCount = 0;

            while (true)
            {
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, 5f),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomBottomSector);
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, 5f),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomTopSector);

                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones-initialDroneCount) { droneCount += droneIncrease; }
            }
        }
    }
}
