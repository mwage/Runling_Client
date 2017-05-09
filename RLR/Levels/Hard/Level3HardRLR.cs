using System.Collections.Generic;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level3HardRLR : ALevelRLR
    {
        public Level3HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(9.5f, 1f, Color.green, moveDelegate: DroneMovement.ChaserMovement, player: Manager.InitializeGameRLR.Player), new int[3] { 1, 8, 16 }, new int[3] { 4, 12, 19 });
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn blue drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.1f, 1),
                new RandomDrone(10, 1, Color.blue, restrictedZone: 0, droneType: DroneType.FlyingOnewayDrone));

            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(15, 1, Color.red, 3, laneArea[0]), 100);
        }
    }
}
