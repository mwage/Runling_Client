using System.Collections.Generic;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level5HardRLR : ALevelRLR
    {
        public Level5HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(6, 2, Color.grey), (int)(12 - i * 0.4f), area: laneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(6, 2, Color.grey), 4, area: laneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(6, 2, Color.grey), 3, area: laneArea[20]);

            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(17, 2, Color.red, 3, laneArea[0]), 70);
        }
    }
}
