using System.Collections;
using MP.TSGame.Drones.Pattern;
using MP.TSGame.Drones.Types;
using TrueSync;

namespace MP.TSGame.SLA.Levels
{
    public class Level8SLA : ALevelSLA
    {
        public Level8SLA(LevelManagerSLA manager) : base(manager)
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
            TrueSyncManager.SyncedStartCoroutine(GenerateGreenDrones(2.5f, 12, 7, 1.5f, DroneColor.DarkGreen, 0.02f, 1.5f, 1, 24));

        }

        private IEnumerator GenerateGreenDrones(float delay, int initialDroneCount, float speed, float size,
            DroneColor color,float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var levelCounter = DroneFactory.LevelCounter;
            var droneCount = 0;

            while (true)
            {
                if (levelCounter != DroneFactory.LevelCounter)
                    yield break;

                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, delay, false, true, -90, 180, changeDirection: true, patternRepeats: 2),
                    new DefaultDrone(speed, size, color), posDelegate: delegate {
                        return new TSVector(0, 0.4f, BoundariesSLA.FlyingSla.BottomBoundary + (0.5f + 1.5f / 2));
                    });

                yield return 2 * delay;

                if (levelCounter != DroneFactory.LevelCounter)
                    yield break;

                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + droneCount, delay, false, false, -90, 180, changeDirection: true, patternRepeats: 2),
                    new DefaultDrone(speed, size, color), posDelegate: delegate {
                        return new TSVector(0, 0.4f, BoundariesSLA.FlyingSla.TopBoundary - (0.5f + 1.5f / 2));
                    });

                yield return 2 * delay;

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