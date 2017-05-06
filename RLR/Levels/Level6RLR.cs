using Assets.Scripts.Drones;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.RLR.Levels
{
    public class Level6RLR : ALevelRLR
    {
        public Level6RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new ChaserDrone(5f, 1f, Color.green, Manager.InitializeGameRLR.Player), new int[3] { 1, 8, 16 }, new int[3] { 4, 12, 19 });
        }

        public override void CreateDrones()
        {
            // Spawn blue drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.1f, 2),
                new RandomDrone(7, 2, Color.blue, restrictedZone: 0, droneType: DroneType.FlyingOnewayDrone), moveDelegate: DroneMovement.SinusoidalMovement);
        }
    }
}
