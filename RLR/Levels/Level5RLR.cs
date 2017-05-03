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
                DroneFactory.SpawnDrones(new RandomBouncingDrone(4f, 1f, Color.grey), (int) (10 - i * 0.4f), area: laneArea[i]);
                DroneFactory.SpawnDrones(new RandomBouncingDrone(4f, 2f, Color.grey), (int) (5 - i * 0.2f), area: laneArea[i]);
            }
        }
    }
}
