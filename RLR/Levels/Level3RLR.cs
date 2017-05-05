using Assets.Scripts.Drones;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level3RLR : ALevelRLR
    {
        public Level3RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            // Spawn blue drones
            DroneFactory.StartCoroutine(GenerateBlueDrones(0.1f, 7, 1, Color.blue, DroneMovement.SinusMovement));

            Area[] laneArea = Manager.GenerateMapRLR.GetDroneSpawnArea();
            DroneFactory.SpawnDrones(new GridDrones(7f, 1f, Color.magenta, 20, 0.04f, false, DroneMovement.SinusMovement), area: laneArea[0]);
            DroneFactory.SpawnDrones(new RandomFlyingBouncingDrone(6f, 1f, Color.red), 50, area: laneArea[0], moveDelegate: DroneMovement.SinusMovement);
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomBouncingDrone(2f, 1f, Color.grey), (int)(6 - i * 0.2f), area: laneArea[i], moveDelegate: DroneMovement.SinusMovement);
            }
            var mines = new GameObject[2];
            for (var i = 0; i < mines.Length; i++)
            {
                mines[i] = DroneFactory.SpawnDrones(new MineDrone(5f, 3f, Color.black), area: laneArea[0], moveDelegate: DroneMovement.SinusMovement);
            }
            MineVariations.AddDelayedStraightFlying360Drones(32, 2f, 2, 8f, 1f, Color.cyan, mines, DroneFactory, moveDelegate: DroneMovement.SinusMovement);
        }

        private IEnumerator GenerateBlueDrones(float delay, float speed, float size, Color color, DroneMovement.MovementDelegate moveDelegate)
        {
            while (true)
            {
                DroneFactory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, new Vector3(0, 0.6f, 0), 
                    DroneDirection.RandomDirection(0)), 2, moveDelegate: moveDelegate);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}