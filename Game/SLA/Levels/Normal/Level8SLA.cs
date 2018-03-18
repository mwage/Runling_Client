using Game.Scripts.Drones.DroneTypes;
using Game.Scripts.Drones.Pattern;
using System.Collections;
using UnityEngine;

namespace Game.Scripts.SLA.Levels.Normal
{
    public class Level8SLA : ALevelSLA
    {
        public Level8SLA(ILevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7, 1f, DroneColor.Blue), 10, 6, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7, 1.5f, DroneColor.Red), 6, 10, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.StartCoroutine(GenerateGreenDrones(2.5f, 12, 7, 1.5f, DroneColor.DarkGreen, 0.02f, 1.5f, 1, 24));

        }

        private IEnumerator GenerateGreenDrones(float delay, int initialDroneCount, float speed, float size,
            DroneColor color,float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var droneCount = 0;

            while (true)
            {
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, delay, false, true, -90, 180, changeDirection: true, patternRepeats: 2),
                    new DefaultDrone(speed, size, color), posDelegate: delegate {
                        return new Vector3(0, 0.4f, BoundariesSLA.FlyingSla.BottomBoundary + (0.5f + 1.5f / 2));
                    });
                yield return new WaitForSeconds(2 * delay);
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, delay, false, false, -90, 180, changeDirection: true, patternRepeats: 2),
                    new DefaultDrone(speed, size, color), posDelegate: delegate {
                        return new Vector3(0, 0.4f, BoundariesSLA.FlyingSla.TopBoundary - (0.5f + 1.5f / 2));
                    });

                yield return new WaitForSeconds(2*delay);

                if (delay > minDelay)
                {
                    delay -= delay * reduceDelay;
                }
                if (droneCount < maxDrones - initialDroneCount)
                {
                    droneCount += droneIncrease;
                }
            }
        }
    }
}