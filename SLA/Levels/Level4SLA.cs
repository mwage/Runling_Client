using System.Collections;
using Drones;
using Drones.DroneTypes;
using Drones.Pattern;
using UnityEngine;

namespace SLA.Levels
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
            DroneFactory.SpawnAndAddDrones(new RandomDrone(5f, 1f, DroneColor.Blue), 10, 4f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(5f, 1.5f, DroneColor.Red), 8, 8f, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.StartCoroutine(GenerateLevel4GreenDrones(5f, 16, 8f, 1.5f, DroneColor.DarkGreen, 0.1f, 1f, 1, 16));
        }

        private IEnumerator GenerateLevel4GreenDrones(float delay, int initialDroneCount, float speed, float size, DroneColor color, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var droneCount = 0;
            while (true)
            {
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + 2 * droneCount),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomBottomSector);
                yield return new WaitForSeconds(delay);

                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + 2 * droneCount),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomTopSector);
                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay*reduceDelay; }
                if (droneCount < maxDrones) { droneCount += droneIncrease; }
            }
        }
    }
}
