using Drones.DroneTypes;
using Drones.Movement;
using Drones.Pattern;

namespace RLR.Levels.Normal
{
    public class Level6RLR : ALevelRLR
    {
        public Level6RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(6, 1f, DroneColor.BrightGreen, moveDelegate: DroneMovement.ChaserMovement), new[] { 1, 8}, new[] { 4, 12});
        }

        public override void CreateDrones()
        {
            // Spawn blue drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.2f, 2),
                new RandomDrone(8, 2, DroneColor.Blue, restrictedZone: 0, droneType: DroneType.FlyingOnewayDrone, moveDelegate: DroneMovement.FixedSinusoidalMovement));
        }
    }
}
