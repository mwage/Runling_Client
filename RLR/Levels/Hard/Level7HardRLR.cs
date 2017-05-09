using System.Collections.Generic;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level7HardRLR : ALevelRLR
    {
        public Level7HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(7, 2, Color.grey), (int)(12 - i * 0.4f), area: laneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(7, 2, Color.grey), 4, area: laneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(7, 2, Color.grey), 3, area: laneArea[20]);

            // Spawn yellow drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.15f, 2),
                new RandomDrone(15, 2, Color.yellow, restrictedZone: 0, droneType: DroneType.FlyingOnewayDrone, moveDelegate: DroneMovement.SinusoidalMovement, sinForce: 80, sinFrequency: 4));
        }
    }
}
