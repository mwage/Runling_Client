using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level11SLA : ALevelSLA
    {
        public Level11SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn drones (dronecount/delay, speed, size, color)
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1.5f, Color.red), 10, 7f);

            // Spawn Mine (speed, size)
            var mine = DroneFactory.SpawnDrones(new MineDrone(5f, 3f, Color.black));
            var mine2 = DroneFactory.SpawnDrones(new MineDrone(5f, 3f, Color.black));
            var mine3 = DroneFactory.SpawnDrones(new MineDrone(5f, 3f, Color.black));

            DroneFactory.StartCoroutine(MineLevel11(32, 8f, 8f, 1f, Color.cyan, mine, mine2, mine3));
        }

        private IEnumerator MineLevel11(int droneCount, float delay, float speed, float size, Color color, GameObject mine, GameObject mine2, GameObject mine3)
        {
            while (true)
            {
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, color, droneCount, false, position: mine.transform.position));
                yield return new WaitForSeconds(delay / 3);
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, color, droneCount, false, position: mine2.transform.position));
                yield return new WaitForSeconds(delay / 3);
                DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, color, droneCount, false, position: mine3.transform.position));
                yield return new WaitForSeconds(delay / 3);
                if (delay > 3f) { delay -= delay * 0.1f; }
            }
        }
    }
}