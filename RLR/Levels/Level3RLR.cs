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

        public override void SetChasers()
        {
            int[] spawnChaser = new int[17];
            int[] destroyChaser = new int[17];
            int[] chaserStartPosition = new int[17];

            for (var i = 0; i < 17; i++)
            {
                spawnChaser[i] = i + 1;
                destroyChaser[i] = i;
                chaserStartPosition[i] = i + 2;
            }
            Manager.RunlingChaser.SetChaserPlatforms(
                new ChaserDrone(5f, 1f, Color.green, Manager.InitializeGameRLR.Player), spawnChaser, destroyChaser, chaserStartPosition);
        }

        public override void CreateDrones()
        {
            // Spawn blue drones
            DroneFactory.StartCoroutine(GenerateBlueDrones(0.1f, 6, 1, Color.blue, DroneMovement.SinusMovement));
        }

        private IEnumerator GenerateBlueDrones(float delay, float speed, float size, Color color, DroneMovement.MovementDelegate moveDelegate)
        {
            while (true)
            {
                DroneFactory.SpawnDrones(new OnewayDrone(speed, size, color, new Vector3(0, 0.6f, 0), 
                    DroneDirection.RandomDirection(0)), 2, moveDelegate: moveDelegate);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}