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
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7f, 1f, Color.blue), 15, 6f, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.StartCoroutine(GenerateLevel6GreenDrones(4f, 24, 7f, 1.5f, Color.green, 0.03f, 1.5f, 2, 48, DroneStartPosition.GetRandomBottomSector));
            DroneFactory.StartCoroutine(GenerateLevel6GreenDrones(5f, 24, 7f, 1.5f, Color.green, 0.04f, 1.5f, 2, 48, DroneStartPosition.GetRandomTopSector));
        }

        private IEnumerator GenerateLevel6GreenDrones(float delay, int initialDroneCount, float speed, float size, Color color,
            float reduceDelay, float minDelay, int droneIncrease, int maxDrones, StartPositionDelegate posDelegate)
        {
            var droneCount = 0;

            while (true)
            {
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, 5f),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, posDelegate: posDelegate);
                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones-initialDroneCount) { droneCount += droneIncrease; }
            }
        }
    }
}
