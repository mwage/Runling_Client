using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class Pat360Drones : APattern
    {
        protected readonly int NumRays;
        protected readonly float? Delay;
        protected readonly bool Repeat;
        protected readonly bool? Clockwise;
        protected readonly float? StartRotation;


        public Pat360Drones(int numRays, float? delay = null, bool? repeat = null, bool? clockwise = null, float? startRotation = null)
        {
            NumRays = numRays;
            Delay = delay;
            Clockwise = clockwise;
            StartRotation = startRotation;
            Repeat = repeat ?? false;
        }

        public override void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null, DroneMovement.MovementDelegate moveDelegate = null)
        {
            if (posDelegate == null)
                posDelegate = delegate { return new Vector3(0, 0.6f, 0); };

            factory.StartCoroutine(Generate360Drones(factory, drone, area, posDelegate, moveDelegate));
        }

        public override void AddPattern(DroneFactory factory, GameObject drone, IDrone addedDrone, Area area, DroneMovement.MovementDelegate moveDelegate = null)
        {
            factory.StartCoroutine(Generate360Drones(factory, addedDrone, area, delegate { return Vector3.zero; }, moveDelegate, drone));
        }


        private IEnumerator Generate360Drones(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate, DroneMovement.MovementDelegate moveDelegate, GameObject parentDrone = null)
        {
            var clockwise = true;
            var startRotation = 0f;
            var position = posDelegate(drone.GetSize(), area);



            // If delay is not null, the drones will go out in a fan motion.  If it is null, all rays will go out at the same time
            if (Delay != null)
            {
                // Set Rotation according to position
                if (posDelegate == DroneStartPosition.GetRandomTopSector)
                {
                    clockwise = position.x < 0;
                    startRotation = (position.x < 0) ? 90f : -90f;
                }
                else if (posDelegate == DroneStartPosition.GetRandomBottomSector)
                {
                    clockwise = position.x >= 0;
                    startRotation = (position.x < 0) ? 90f : -90f;
                }
                else if (posDelegate == DroneStartPosition.GetRandomLeftSector)
                {
                    clockwise = position.z < 0;
                    startRotation = (position.z < 0) ? 0f : 180f;
                }
                else if (posDelegate == DroneStartPosition.GetRandomRightSector)
                {
                    clockwise = position.z >= 0;
                    startRotation = (position.z < 0) ? 0f : 180f;
                }

                clockwise = Clockwise ?? clockwise;
                startRotation = StartRotation ?? startRotation;
            }

            do
            {
                for (var i = 0; i < NumRays; i++)
                {
                    if (parentDrone != null)
                    {
                        position = parentDrone.transform.position;
                    }
                    // spawn new drone in set position, direction and dronespeed
                    var rotation = startRotation + (clockwise ? 1 : -1) * (360f * i / NumRays);
                    factory.SpawnDrones(new OnewayDrone(drone.GetSpeed(), drone.GetSize(), drone.GetColor(), position, rotation) , moveDelegate: moveDelegate);

                    if (Delay != null)
                        yield return new WaitForSeconds(Delay.Value / (NumRays));
                }
            } while (Repeat && Delay != null);
        }
    }
}
