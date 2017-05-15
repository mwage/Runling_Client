﻿using System.Collections;
using Assets.Scripts.Drones;
using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level9SLA : ALevelSLA
    {
        public Level9SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7f, 1.5f, Color.red), 15, 9f, BoundariesSLA.BouncingSla);

            // Spawn Chaser Drone
            DroneFactory.SpawnDrones(new DefaultDrone(7f, 1.1f, Color.yellow, moveDelegate: DroneMovement.ChaserMovement, player: GameControl.Player));
        
            // Spawn Green Drones
            DroneFactory.StartCoroutine(GenerateLevel9GreenDrones(4f, 16, 8f, 1.5f, Color.green, 0.1f, 1f, 1, 32));
        }

        IEnumerator GenerateLevel9GreenDrones(float delay, int initialDroneCount, float speed, float size, Color color, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
        {
            var droneCount = 0;
            while (true)
            {
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + 2 * droneCount),
                    new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomBottomSector);
                yield return new WaitForSeconds(delay);
                DroneFactory.SetPattern(new Pat360Drones(initialDroneCount + 2 * droneCount), new DefaultDrone(speed, size, color), BoundariesSLA.FlyingSla, DroneStartPosition.GetRandomTopSector);
                yield return new WaitForSeconds(delay);
                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones-initialDroneCount) { droneCount += droneIncrease; }
            }
        }
    }
}