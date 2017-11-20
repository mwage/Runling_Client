using System.Collections;
using Drones;
using Drones.DroneTypes;
using Drones.Pattern;
using UnityEngine;

namespace SLA.Levels
{
    public class Level4SLA : ALevelSLA
    {
        public Level4SLA(ILevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(6, 1f, DroneColor.Blue), 12, 5, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(6, 1.5f, DroneColor.Red), 6, 9, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.StartCoroutine(GenerateLevel4GreenDrones(4, 16, 8, 1.5f, DroneColor.DarkGreen, 0.05f, 1, 1, 16));
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
