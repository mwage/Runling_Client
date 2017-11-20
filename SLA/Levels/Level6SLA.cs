using System.Collections;
using Drones;
using Drones.DroneTypes;
using Drones.Pattern;
using UnityEngine;

namespace SLA.Levels
{
    public class Level6SLA : ALevelSLA
    {
        public Level6SLA(ILevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7, 1f, DroneColor.Blue), 13, 8, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.StartCoroutine(GenerateLevel6GreenDrones(4f, 24, 7f, 1.5f, DroneColor.DarkGreen, 0.01f, 1.5f, 1, 40, DroneStartPosition.GetRandomBottomSector));
            DroneFactory.StartCoroutine(GenerateLevel6GreenDrones(5f, 24, 7f, 1.5f, DroneColor.DarkGreen, 0.02f, 1.5f, 1, 40, DroneStartPosition.GetRandomTopSector));
        }

        private IEnumerator GenerateLevel6GreenDrones(float delay, int initialDroneCount, float speed, float size, DroneColor color,
            float reduceDelay, float minDelay, int droneIncrease, int maxDrones, StartPositionDelegate posDelegate)
        {
            var droneCount = 0;

            while (true)
            {
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, 5f),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, posDelegate);
                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones-initialDroneCount) { droneCount += droneIncrease; }
            }
        }
    }
}
