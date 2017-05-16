using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level8RLR : ALevelRLR
    {
        public Level8RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(5, 2f, DroneColor.Grey), (int)(12 - i * 0.4f), area: laneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(5, 2f, DroneColor.Grey), 4, area: laneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(5, 2f, DroneColor.Grey), 3, area: laneArea[20]);

            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(13, 2, DroneColor.Red, 3, laneArea[0]), 60);
        }
    }
}
