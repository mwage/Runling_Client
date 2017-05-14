using System.Collections.Generic;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level2HardRLR : ALevelRLR
    {
        public Level2HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(9, 1f, Color.green, moveDelegate: DroneMovement.ChaserMovement, player: Manager.InitializeGameRLR.Player), new int[3] { 1, 8, 16 }, new int[3] { 4, 12, 19 });
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(5, 1f, Color.grey), (int)(15 - i * 0.6f), area: laneArea[i]);
            }

            // Spawn blue drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.1f, 2),
                new RandomDrone(10, 1, Color.blue, restrictedZone: 0, droneType: DroneType.FlyingOnewayDrone, moveDelegate: DroneMovement.CurvedMovement));

            // Middle reds
            DroneFactory.SpawnDrones(new RedDrone(500, 1, Color.red, 1000, laneArea[19], 5, true, DroneType.MineDroneBouncing), 5);
            DroneFactory.SpawnDrones(new RedDrone(500, 1, Color.red, 1000, laneArea[20], 5, true, DroneType.MineDroneBouncing), 4);
        }
    }
}
