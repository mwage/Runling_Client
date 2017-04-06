using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level8SLA : ALevelSLA
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
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(6f, 1f, Color.blue), 10, 4f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(6f, 1.5f, Color.red), 8, 8f, BoundariesSLA.BouncingSla);
            
            // Spawn Green Drones
            DroneFactory.StartCoroutine(GreenDronesLevel8(5f, 12, 7f, 1.5f, Color.green, 0.05f, 2f, 1, 16, 2.5f, 0.03f, 1.5f));
        }


        IEnumerator GreenDronesLevel8(float delay, int initialDroneCount, float speed, float size, Color color, float reduceDelay, float minDelay, int droneIncrease, int maxDrones, float durationCycle, float? reduceCycleDuration = null, float? minCycleDuration = null)
        {
            Vector3 startPos = new Vector3(0, 0.6f, BoundariesSLA.FlyingSla.BottomBoundary + (0.5f + size / 2));
            Vector3 startPos2 = new Vector3(0, 0.6f, BoundariesSLA.FlyingSla.TopBoundary - (0.5f + size / 2));
            int droneCount = 0;
        
            while (true)
            {
                DroneFactory.StartCoroutine(IGreenDronesLevel8(initialDroneCount + droneCount, speed, size, color, startPos, -90, durationCycle, reduceCycleDuration, minCycleDuration));
                yield return new WaitForSeconds(delay);
                DroneFactory.StartCoroutine(IGreenDronesLevel8(initialDroneCount + droneCount, speed, size, color, startPos2, 90, durationCycle, reduceCycleDuration, minCycleDuration));
                yield return new WaitForSeconds(delay);

                if (delay > minDelay) { delay -= delay * reduceDelay; }
                if (droneCount < maxDrones) { droneCount += droneIncrease; }
                if (reduceCycleDuration != null && minCycleDuration != null) { durationCycle = durationCycle > minCycleDuration.Value ? durationCycle - durationCycle * reduceCycleDuration.Value : minCycleDuration.Value; }

            }
        }

        IEnumerator IGreenDronesLevel8(int droneCount, float speed, float size, Color color, Vector3 startPos, float startRotation, float durationCycle, float? reduceCycleDuration = null, float? minCycleDuration = null)
        {
            float rotation;
            float delay = durationCycle / droneCount;
            
            for (int i = 0; i < droneCount; i++)
            {
                rotation = startRotation + 180 * i / droneCount;
                DroneFactory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, startPos, rotation));
                yield return new WaitForSeconds(delay);
            }

            for (int i = 0; i < droneCount; i++)
            {
                rotation = startRotation + 180 - 180 * i / droneCount;
                DroneFactory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, startPos, rotation));
                yield return new WaitForSeconds(delay);
            }

        }
    }
}