using Assets.Scripts.Drones;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.RLR.Levels
{
    public class Level6RLR : ALevelRLR
    {
        public Level6RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new ChaserDrone(5f, 1f, Color.green, Manager.InitializeGameRLR.Player), new int[3] { 1, 8, 16 }, new int[3] { 4, 12, 19 });
        }

        public override void CreateDrones()
        {
            // Spawn blue drones
            DroneFactory.StartCoroutine(GenerateBlueDrones(0.1f, 7, 2, Color.blue));
        }

        private IEnumerator GenerateBlueDrones(float delay, float speed, float size, Color color)
        {
            while (true)
            {
                DroneFactory.SpawnDrones(new OnewayDrone(speed, size, color, direction: DroneDirection.RandomDirection(0)), 2);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
