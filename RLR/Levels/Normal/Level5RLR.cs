using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level5RLR : ALevelRLR
    {
        public Level5RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(4.5f, 1f, DroneColor.Grey), (int) (10 - i * 0.35f), area: laneArea[i]);
                DroneFactory.SpawnDrones(new RandomDrone(4.5f, 2f, DroneColor.Grey), (int) (5 - i * 0.15f), area: laneArea[i]);
            }
            DroneFactory.SpawnDrones(new RedDrone(500, 1, DroneColor.Red, 1000, laneArea[19], 5, true, DroneType.MineDroneBouncing), 5);
            DroneFactory.SpawnDrones(new RedDrone(500, 1, DroneColor.Red, 1000, laneArea[20], 5, true, DroneType.MineDroneBouncing), 4);
        }
    }
}
