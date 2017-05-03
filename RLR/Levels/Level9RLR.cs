using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level9RLR : ALevelRLR
    {
        public Level9RLR(LevelManagerRLR manager) : base(manager)
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

            // Spawn red drones
            DroneFactory.SpawnDrones(new RandomFlyingBouncingDrone(6f, 2f, Color.red), 100, area: laneArea[0]);

            // Spawn blue drones
            DroneFactory.SpawnDrones(new StraightFlying360Drone(7, 2, Color.blue, 16, 8, true, true, 270));
            DroneFactory.SpawnDrones(new StraightFlying360Drone(7, 2, Color.blue, 16, 8, true, true, 90));
        }
    }
}
