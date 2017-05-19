using Drones.DroneTypes;
using Drones.Movement;
using Drones.Pattern;

namespace RLR.Levels.Hard
{
    public class Level8HardRLR : ALevelRLR
    {
        public Level8HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(9f, 1f, DroneColor.BrightGreen, moveDelegate: DroneMovement.ChaserMovement), new[] { 1, 8}, new[] { 4, 12});
        }

        public override void CreateDrones()
        {
            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(17, 2, DroneColor.Red, 3, LaneArea[0]), 60);

            // Spawn yellow drones
            DroneFactory.SetPattern(new Pat360Drones(32, 3, true, startRotation: -90),
                new DefaultDrone(12, 2, DroneColor.Golden, moveDelegate: DroneMovement.SinusoidalMovement, sinForce: 60, sinFrequency: 5));
        }
    }
}
