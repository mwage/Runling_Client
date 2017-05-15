using System.Collections.Generic;
using Assets.Scripts.Drones;
using Assets.Scripts.Launcher;
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
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(9f, 1f, Color.green, moveDelegate: DroneMovement.ChaserMovement), new int[2] { 1, 8}, new int[2] { 4, 12});
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(17, 2, Color.red, 3, laneArea[0]), 80);

            // Spawn yellow drones
            DroneFactory.SetPattern(new Pat360Drones(32, 3, true, startRotation: -90),
                new DefaultDrone(15, 2, Color.yellow, moveDelegate: DroneMovement.FixedCosinusoidalMovement, sinForce: 80, sinFrequency: 5.5f));
        }
    }
}
