using System.Collections.Generic;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level4HardRLR : ALevelRLR
    {
        public Level4HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(6, 2f, Color.grey), (int)(12 - i * 0.4f), area: laneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(6, 2f, Color.grey), 4, area: laneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(6, 2f, Color.grey), 3, area: laneArea[20]);

            // Spawn yellow drones
            DroneFactory.SetPattern(new Pat360Drones(32, repeat: true, pulseDelay: 5),
                new DefaultDrone(13, 2, Color.blue));
        }
    }
}
