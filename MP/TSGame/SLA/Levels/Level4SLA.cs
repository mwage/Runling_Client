using System.Collections;
using MP.TSGame.Drones;
using MP.TSGame.Drones.Pattern;
using MP.TSGame.Drones.Types;
using TrueSync;

namespace MP.TSGame.SLA.Levels
{
    public class Level4SLA : ALevelSLA
    {
        public Level4SLA(LevelManagerSLA manager) : base(manager)
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
            TrueSyncManager.SyncedStartCoroutine(GenerateLevel4GreenDrones(4, 16, 8, 1.5f, DroneColor.DarkGreen, 0.05f, 1, 1, 16));
        }

        private IEnumerator GenerateLevel4GreenDrones(float delay, int initialDroneCount, float speed, float size, DroneColor color, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
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
                    yield break;;

                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + 2 * droneCount),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomTopSector);
                yield return delay;

                if (delay > minDelay) { delay -= delay*reduceDelay; }
                if (droneCount < maxDrones) { droneCount += droneIncrease; }
            }
        }
    }
}
