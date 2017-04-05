using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level7SLA : ALevelSLA
    {
        public Level7SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        private GameObject _mine;

        public override void CreateDrones()
        {
            // Spawn drones (dronecount/delay, speed, size, color)
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(6f, 1.5f, Color.red), 15, 6f);
            
            // Spawn Mine (speed, size)
            _mine = DroneFactory.SpawnDrones(new MineDrone(5f, 3f, Color.black));

            // Spawn Green drones (dronecount, delay, speed, size, color)
            DroneFactory.StartCoroutine(GreenDronesLevel7(64, 5f, 8f, 1.3f, Color.cyan));
        }

        IEnumerator GreenDronesLevel7(int droneCount, float delay, float droneSpeed, float size, Color color)
        {
            while (true)
            {
                Renderer rend;
                Vector3 scale;

                for (int i = 0; i < droneCount; i++)
                {
                    // spawn new drone in set position, direction and dronespeed
                    GameObject newDrone = Object.Instantiate(DroneFactory.FlyingOnewayDrone, _mine.transform.position, Quaternion.Euler(0, i * 720 / droneCount, 0));

                    // adjust drone color and size
                    rend = newDrone.GetComponent<Renderer>();
                    rend.material.color = color;
                    scale = newDrone.transform.localScale;
                    scale.x *= size;
                    scale.z *= size;
                    newDrone.transform.localScale = scale;

                    // move drone
                    MoveDrone.MoveStraight(newDrone, droneSpeed);
                    yield return new WaitForSeconds(delay/droneCount);
                }
                for (int i = 0; i < droneCount; i++)
                {
                    // spawn new drone in set position, direction and dronespeed
                    GameObject newDrone = Object.Instantiate(DroneFactory.FlyingOnewayDrone, _mine.transform.position, Quaternion.Euler(0, -i * 720 / droneCount, 0));

                    // adjust drone color and size
                    rend = newDrone.GetComponent<Renderer>();
                    rend.material.color = color;
                    scale = newDrone.transform.localScale;
                    scale.x *= size;
                    scale.z *= size;
                    newDrone.transform.localScale = scale;

                    // move drone
                    MoveDrone.MoveStraight(newDrone, droneSpeed);
                    yield return new WaitForSeconds(delay/droneCount);
                    if (delay < 3)delay -= 0.05f * delay;
                }
            }
        }
    }
}