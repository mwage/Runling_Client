using Assets.Scripts.Drones;
using UnityEngine;
using Assets.Scripts.Launcher;

namespace Assets.Scripts.RLR.Levels
{
    public class Level6RLR : ALevelRLR
    {
        public Level6RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(6, 1f, Color.green, moveDelegate: DroneMovement.ChaserMovement), new int[2] { 1, 8}, new int[2] { 4, 12});
        }

        public override void CreateDrones()
        {
            // Spawn blue drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.2f, 2),
                new RandomDrone(8, 2, Color.blue, restrictedZone: 0, droneType: DroneType.FlyingOnewayDrone, moveDelegate: DroneMovement.FixedCosinusoidalMovement));
        }
    }
}
