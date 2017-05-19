using Drones.DroneTypes;
using Drones.Movement;
using Drones.Pattern;

namespace RLR.Levels.Hard
{
    public class Level3HardRLR : ALevelRLR
    {
        public Level3HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(9, 1f, DroneColor.BrightGreen, moveDelegate: DroneMovement.ChaserMovement), new[] { 1, 8, 16 }, new[] { 4, 12, 19 });
        }

        public override void CreateDrones()
        {
            // Spawn blue drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.1f, 2),
                new RandomDrone(10, 1, DroneColor.Blue, restrictedZone: 0, droneType: DroneType.FlyingOnewayDrone));

            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(15, 1, DroneColor.Red, 3, LaneArea[0]), 100);
        }
    }
}
