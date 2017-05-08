using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level1RLR : ALevelRLR
    {
        public Level1RLR(LevelManagerRLR manager) : base(manager)
        {
        }
        

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length-2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(2f, 1f, Color.grey), (int)(15-i*0.6f), area: laneArea[i]);
            }

            DroneFactory.SpawnDrones(new RedDrone(500, 1, Color.red, 1000, laneArea[19], 5, true, DroneType.MineDroneBouncing), 5);
            DroneFactory.SpawnDrones(new RedDrone(500, 1, Color.red, 1000, laneArea[20], 5, true, DroneType.MineDroneBouncing), 4);
        }
    }
}
