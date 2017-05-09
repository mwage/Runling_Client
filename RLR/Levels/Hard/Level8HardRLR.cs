using System.Collections.Generic;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level8HardRLR : ALevelRLR
    {
        public Level8HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(9.5f, 1f, Color.green, moveDelegate: DroneMovement.ChaserMovement, player: Manager.InitializeGameRLR.Player), new int[2] { 1, 8}, new int[2] { 4, 12});
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(17, 2, Color.red, 3, laneArea[0]), 70);

            // Spawn yellow drones
            DroneFactory.SetPattern(new Pat360Drones(32, repeat: true, pulseDelay: 4),
                new DefaultDrone(15, 2, Color.yellow, moveDelegate: DroneMovement.SinusoidalMovement));
        }
    }
}
