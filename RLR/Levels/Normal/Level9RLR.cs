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
                DroneFactory.SpawnDrones(new RandomDrone(5, 2f, DroneColor.Grey), (int)(9 - i * 0.35f), area: laneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(5, 2f, DroneColor.Grey), 3, area: laneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(5, 2f, DroneColor.Grey), 2, area: laneArea[20]);

            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(13, 2, DroneColor.Red, 3, laneArea[0]), 50);

            // Spawn blue drones
            DroneFactory.SetPattern(new Pat360Drones(32, 10, true, true, 270), new DefaultDrone(8, 2, DroneColor.Blue));
            DroneFactory.SetPattern(new Pat360Drones(32, 10, true, true, 90), new DefaultDrone(8, 2, DroneColor.Blue));
        }
    }
}
