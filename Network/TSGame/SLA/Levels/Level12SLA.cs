using System.Collections;
using MP.TSGame.Drones;
using MP.TSGame.Drones.Pattern;
using MP.TSGame.Drones.Types;
using TrueSync;

namespace MP.TSGame.SLA.Levels
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
            TrueSyncManager.SyncedStartCoroutine(GenerateLevel12GreenDrones(5f, 9f, 1.2f, DroneColor.Cyan, 32, 0.02f, 2.5f, 1, 48));
        }

        private IEnumerator GenerateLevel12GreenDrones(float delay, float speed, float size, DroneColor color, int initialDroneCount, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var levelCounter = DroneFactory.LevelCounter;
            var droneCount = 0;

            while (true)
            {
                if (levelCounter != DroneFactory.LevelCounter)
                    yield break;

                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, 5f),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomBottomSector);
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, 5f),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomTopSector);

                yield return delay * 6 / 5;

                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones-initialDroneCount) { droneCount += droneIncrease; }
            }
        }
    }
}
