using System.Collections;
using MP.TSGame.Drones;
using MP.TSGame.Drones.Pattern;
using MP.TSGame.Drones.Types;
using TrueSync;

namespace MP.TSGame.SLA.Levels
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
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7, 1f, DroneColor.Blue), 13, 8, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            TrueSyncManager.SyncedStartCoroutine(GenerateLevel6GreenDrones(4f, 24, 7f, 1.5f, DroneColor.DarkGreen, 0.01f, 1.5f, 1, 40, DroneStartPosition.GetRandomBottomSector));
            TrueSyncManager.SyncedStartCoroutine(GenerateLevel6GreenDrones(5f, 24, 7f, 1.5f, DroneColor.DarkGreen, 0.02f, 1.5f, 1, 40, DroneStartPosition.GetRandomTopSector));
        }

        private IEnumerator GenerateLevel6GreenDrones(float delay, int initialDroneCount, float speed, float size, DroneColor color,
            float reduceDelay, float minDelay, int droneIncrease, int maxDrones, StartPositionDelegate posDelegate)
        {
            var levelCounter = DroneFactory.LevelCounter;
            var droneCount = 0;

            while (true)
            {
                if (levelCounter != DroneFactory.LevelCounter)
                    yield break;

                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, 5f),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, posDelegate);
                yield return delay;
                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones-initialDroneCount) { droneCount += droneIncrease; }
            }
        }
    }
}
