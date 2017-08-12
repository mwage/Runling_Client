using System.Collections;
using MP.TSGame.Drones;
using MP.TSGame.Drones.Movement;
using MP.TSGame.Drones.Pattern;
using MP.TSGame.Drones.Types;
using MP.TSGame.Players;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.SLA.Levels
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

            // Spawn Chaser Drones
            foreach (Transform child in GameObject.Find("Player").transform)
            {
                DroneFactory.SpawnDrones(new DefaultDrone(6, 1, DroneColor.Golden, moveDelegate: DroneMovement.ChaserMovement, player: child.gameObject));
            }
        
            // Spawn Green Drones
            TrueSyncManager.SyncedStartCoroutine(GenerateLevel9GreenDrones(4, 16, 8f, 1.5f, DroneColor.DarkGreen, 0.05f, 1f, 1, 32));
        }

        private IEnumerator GenerateLevel9GreenDrones(float delay, int initialDroneCount, float speed, float size, DroneColor color, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var levelCounter = DroneFactory.LevelCounter;
            var droneCount = 0;

            while (true)
            {
                if (levelCounter != DroneFactory.LevelCounter)
                    yield break;

                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + 2 * droneCount),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomBottomSector);
                yield return delay;

                if (levelCounter != DroneFactory.LevelCounter)
                    yield break;

                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + 2 * droneCount), new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomTopSector);
                yield return delay;

                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones-initialDroneCount) { droneCount += droneIncrease; }
            }
        }
    }
}