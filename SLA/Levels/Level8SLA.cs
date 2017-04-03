using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level8SLA : ALevel
    {
        public Level8SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn drones (dronecount/delay, speed, size, color)
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(6f, 1f, Color.blue), 10, 4f);
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(6f, 1.5f, Color.red), 8, 8f);
            
            // Spawn green drones (initial delay, inbetween delay, size)
            DroneFactory.StartCoroutine(GreenDronesLevel8(5f, 1.5f, Color.green));
        }

        IEnumerator GreenDronesLevel8(float delay, float size, Color color)
        {
            Vector3 startPos = new Vector3(0, 0.6f, BoundariesSLA.FlyingSla.BottomBoundary + (0.5f + size / 2));
            Vector3 startPos2 = new Vector3(0, 0.6f, BoundariesSLA.FlyingSla.TopBoundary - (0.5f + size / 2));
            int droneCount = 12;
        
            while (true)
            {
                DroneFactory.StartCoroutine(IGreenDronesLevel8(droneCount, 7f, size, color, startPos, -90));
                yield return new WaitForSeconds(delay);
                DroneFactory.StartCoroutine(IGreenDronesLevel8(droneCount, 7f, size, color, startPos2, 90));
                yield return new WaitForSeconds(delay);

                if (delay > 2f) { delay -= delay * 0.05f; }
                if (droneCount < 32) { droneCount++; }
            }
        }

        IEnumerator IGreenDronesLevel8(int droneCount, float droneSpeed, float size, Color color, Vector3 startPos, float startRotation)
        {
            Renderer rend;
            Vector3 scale;
            float rotation;
            float delay = 2.5f / droneCount;


            for (int i = 0; i < droneCount; i++)
            {
                // spawn new drone in set position, direction and dronespeed
                rotation = startRotation + 180 * i / droneCount;
                var newDrone = Object.Instantiate(DroneFactory.FlyingOnewayDrone, startPos, Quaternion.Euler(0, rotation, 0));

                // adjust drone color and size
                rend = newDrone.GetComponent<Renderer>();
                rend.material.color = color;
                scale = newDrone.transform.localScale;
                scale.x *= size;
                scale.z *= size;
                newDrone.transform.localScale = scale;

                // move drone
                MoveDrone.MoveStraight(newDrone, droneSpeed);
                yield return new WaitForSeconds(delay);
            }

            for (int i = 0; i < droneCount; i++)
            {
                // spawn new drone in set position, direction and dronespeed
                rotation = startRotation + 180 - 180 * i / droneCount;
                var newDrone = Object.Instantiate(DroneFactory.FlyingOnewayDrone, startPos, Quaternion.Euler(0, rotation, 0));

                // adjust drone color and size
                rend = newDrone.GetComponent<Renderer>();
                rend.material.color = color;
                scale = newDrone.transform.localScale;
                scale.x *= size;
                scale.z *= size;
                newDrone.transform.localScale = scale;

                // move drone
                MoveDrone.MoveStraight(newDrone, droneSpeed);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}