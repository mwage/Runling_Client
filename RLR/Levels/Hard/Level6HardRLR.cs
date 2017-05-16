using System.Collections.Generic;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level6HardRLR : ALevelRLR
    {
        public Level6HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        protected override float SetAirCollider()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(6, 2f, DroneColor.Grey), (int)(11 - i * 0.4f), area: laneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(6, 2f, DroneColor.Grey), 4, area: laneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(6, 2f, DroneColor.Grey), 3, area: laneArea[20]);

            // Spawn yellow drones
            DroneFactory.SetPattern(new Pat360Drones(40, repeat: true, pulseDelay: 6),
                new DefaultDrone(15, 2, DroneColor.Golden, moveDelegate: DroneMovement.CurvedMovement, curving: 7));
        }
    }
}
