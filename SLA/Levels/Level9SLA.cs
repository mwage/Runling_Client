using System.Collections;
using Drones;
using Drones.DroneTypes;
using Drones.Movement;
using Drones.Pattern;
using UnityEngine;

namespace SLA.Levels
{
    public class Level9SLA : ALevelSLA
    {
        public Level9SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7, 1.5f, DroneColor.Red), 12, 7, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.StartCoroutine(GenerateLevel9GreenDrones(4, 16, 8f, 1.5f, DroneColor.DarkGreen, 0.05f, 1f, 1, 32));

            // Spawn Chaser Drone
            Manager.ControlSLA.PlayerManager.Chaser.AddRange(DroneFactory.SpawnDrones(new DefaultDrone (6, 1.1f, DroneColor.Golden, moveDelegate: DroneMovement.ChaserMovement, chaserTarget: Manager.ControlSLA.PlayerManager)));
        }

        private IEnumerator GenerateLevel9GreenDrones(float delay, int initialDroneCount, float speed, float size, DroneColor color, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var droneCount = 0;
            while (true)
            {
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + 2 * droneCount),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomBottomSector);
                yield return new WaitForSeconds(delay);
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + 2 * droneCount), new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomTopSector);
                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones-initialDroneCount) { droneCount += droneIncrease; }
            }
        }
    }
}