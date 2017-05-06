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
            DroneFactory.SpawnDrones(new RandomBouncingDrone(6f, 2f, Color.red, DroneType.FlyingBouncingDrone), 70, area: laneArea[0]);

            // Spawn blue drones
            DroneFactory.SetPattern(new Pat360Drones(16, 8, true, true, 270), new OnewayDrone(6, 2, Color.blue));
            DroneFactory.SetPattern(new Pat360Drones(16, 8, true, true, 90), new OnewayDrone(6, 2, Color.blue));
        }
    }
}
