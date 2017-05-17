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
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(9f, 1f, DroneColor.DarkGreen, moveDelegate: DroneMovement.ChaserMovement), new[] { 1, 8}, new[] { 4, 12});
        }

        public override void CreateDrones()
        {
            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(17, 2, DroneColor.Red, 3, LaneArea[0]), 80);

            // Spawn yellow drones
            DroneFactory.SetPattern(new Pat360Drones(32, 3, true, startRotation: -90),
                new DefaultDrone(15, 2, DroneColor.Golden, moveDelegate: DroneMovement.FixedCosinusoidalMovement, sinForce: 80, sinFrequency: 5.5f));
        }
    }
}
