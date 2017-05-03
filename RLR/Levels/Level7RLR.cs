using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level7RLR : ALevelRLR
    {
        public Level7RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomBouncingDrone(4f, 2f, Color.grey), (int)(10 - i * 0.4f), area: laneArea[i]);
            }

            // Spawn blue drones
            DroneFactory.SpawnDrones(new StraightFlying360Drone(6, 2, Color.blue, 16, 8, true, true, 270));
        }
    }
}
