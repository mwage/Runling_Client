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
            DroneFactory.StartCoroutine(GenerateBlueDrones(0.1f, 6, 1, Color.blue));
        }

        private IEnumerator GenerateBlueDrones(float delay, float speed, float size, Color color)
        {
            while (true)
            {
                DroneFactory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, new Vector3(0, 0.6f, 0), 
                    DroneDirection.RandomDirection(0)), 2);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}