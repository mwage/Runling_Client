using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Drones {
    public class PatContinuousSpawn : APattern
    {
        protected readonly float Delay;
        protected readonly int DroneCount;

        public PatContinuousSpawn(float delay, int droneCount)
        {
            Delay = delay;
            DroneCount = droneCount;
        }

        public override void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null)
        {
            if (posDelegate == null)
                posDelegate = delegate { return new Vector3(0, 0.6f, 0); };

            factory.StartCoroutine(GenerateDrones(factory, drone, posDelegate));
        }

        public override void AddPattern(DroneFactory factory, GameObject drone, IDrone addedDrone, Area area)
        {

            factory.StartCoroutine(GenerateDrones(factory, addedDrone, delegate { return Vector3.zero; }, drone));
        }

        IEnumerator GenerateDrones(DroneFactory factory, IDrone drone, StartPositionDelegate posDelegate, GameObject parentDrone = null)
        {
            var addPattern = parentDrone != null;
            while (true)
            {
                if (parentDrone == null && addPattern) { yield break; }
                if (parentDrone != null)
                {
                    factory.SpawnDrones( new RandomDrone(drone.GetSpeed(), drone.GetSize(), drone.GetColor(), drone.GetDroneType()), DroneCount, posDelegate: delegate
                    {
                        return parentDrone.transform.position;
                    });
                }
                else
                {
                    factory.SpawnDrones(drone, DroneCount, posDelegate: posDelegate);
                }
                yield return new WaitForSeconds(Delay);
            }
        }
    }
}
