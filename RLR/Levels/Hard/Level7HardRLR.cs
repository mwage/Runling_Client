using Drones.DroneTypes;
using Drones.Movement;
using Drones.Pattern;

namespace RLR.Levels.Hard
{
    public class Level7HardRLR : ALevelRLR
    {
        public Level7HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            // Spawn bouncing drones
            for (var i = 1; i < LaneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(6, 2, DroneColor.Grey), (int)(11 - i * 0.4f), area: LaneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(6, 2, DroneColor.Grey), 4, area: LaneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(6, 2, DroneColor.Grey), 3, area: LaneArea[20]);

            // Spawn yellow drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.1f, 1),
                new RandomDrone(11, 2, DroneColor.Golden, restrictedZone: 0, droneType: DroneType.FlyingOnewayDrone, moveDelegate: DroneMovement.FixedSinusoidalMovement, sinForce: 50, sinFrequency: 5.5f));
        }
    }
}
