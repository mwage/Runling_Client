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
                DroneFactory.SpawnDrones(new RandomBouncingDrone(2f, 1f, Color.grey), (int)(15-i*0.6f), area: laneArea[i]);
                DroneFactory.SpawnDrones(new RandomBouncingDrone(2f, 1f, Color.red), (int)(15 - i * 0.6f), area: laneArea[i], moveDelegate: DroneMovement.SinusMovement); 
            }
        }
    }
}
