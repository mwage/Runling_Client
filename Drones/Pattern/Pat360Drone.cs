using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class Pat360Drones : APattern
    {
        protected readonly float? Delay;
        protected readonly bool Repeat;
        protected readonly bool? Clockwise;
        protected readonly bool ChangeDirection;
        protected readonly float? StartRotation;
        protected readonly float MaxRotation;
        protected readonly float? ReducePulseDelay;
        protected readonly float MinPulseDelay;
        protected float? PulseDelay;
        protected float? InitialDelay;
        protected int NumRays;
        protected readonly int AddRays;
        protected readonly int MaxRays;
        protected readonly float? ReduceDelay;
        protected readonly float MinDelay;
        protected int? PatternRepeats;

        public Pat360Drones(int numRays, float? delay = null, bool? repeat = null, bool? clockwise = null, 
            float? startRotation = null, float? maxRotation = null,float? pulseDelay = null, float? reducePulseDelay = null, 
            float? minPulseDelay = null, float? initialDelay = null, bool? changeDirection = null, int? addRays = null, int? maxRays = null, 
            int? patternRepeats = null, float? reduceDelay = null, float? minDelay = null)
        {
            NumRays = numRays;
            Delay = delay;
            Clockwise = clockwise;
            StartRotation = startRotation;
            Repeat = repeat ?? false;
            PulseDelay = pulseDelay;
            MaxRotation = maxRotation ?? 360;
            ReducePulseDelay = reducePulseDelay;
            MinPulseDelay = minPulseDelay ?? 1;
            InitialDelay = initialDelay;
            ChangeDirection = changeDirection ?? false;
            AddRays = addRays ?? 0;
            MaxRays = maxRays ?? NumRays*2;
            ReduceDelay = reduceDelay;
            MinDelay = minDelay ?? 2;
            PatternRepeats = patternRepeats;
        }

        public override void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null)
        {
            if (posDelegate == null)
                posDelegate = delegate { return new Vector3(0, 0.6f, 0); };

            factory.StartCoroutine(Generate360Drones(factory, drone, area, posDelegate));
        }

        public override void AddPattern(DroneFactory factory, GameObject drone, IDrone addedDrone, Area area)
        {
            factory.StartCoroutine(Generate360Drones(factory, addedDrone, area, delegate { return Vector3.zero; }, drone));
        }


        private IEnumerator Generate360Drones(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate, GameObject parentDrone = null)
        {
            var clockwise = true;
            var startRotation = 0f;
            var position = posDelegate(drone.Size, area);
            var addPattern = parentDrone != null;

            // If delay is not null, the drones will go out in a fan motion.  If it is null, all rays will go out at the same time
            if (Delay != null)
            {
                // Set Rotation according to position
                if (posDelegate == DroneStartPosition.GetRandomTopSector)
                {
                    clockwise = position.x < 0;
                    startRotation = position.x < 0 ? 90f : -90f;
                }
                else if (posDelegate == DroneStartPosition.GetRandomBottomSector)
                {
                    clockwise = position.x >= 0;
                    startRotation = position.x < 0 ? 90f : -90f;
                }
                else if (posDelegate == DroneStartPosition.GetRandomLeftSector)
                {
                    clockwise = position.z < 0;
                    startRotation = (position.z < 0) ? 0f : 180f;
                }
                else if (posDelegate == DroneStartPosition.GetRandomRightSector)
                {
                    clockwise = position.z >= 0;
                    startRotation = position.z < 0 ? 0f : 180f;
                }

                clockwise = Clockwise ?? clockwise;
                startRotation = StartRotation ?? startRotation;
            }

            var midRotaion = clockwise ? startRotation + MaxRotation / 2 : startRotation - MaxRotation / 2;

            if (InitialDelay != null)
            {
                yield return new WaitForSeconds(InitialDelay.Value);
            }

            do
            {
                do
                {
                    for (var i = 0; i < NumRays; i++)
                    {
                        if (parentDrone == null && addPattern)
                        {
                            yield break;
                        }
                        if (parentDrone != null)
                        {
                            position = parentDrone.transform.position;
                        }
                        // spawn new drone in set position, direction and dronespeed
                        var rotation = startRotation + (clockwise ? 1 : -1) * (MaxRotation * i / NumRays);
                        factory.SpawnDrones(new DefaultDrone(drone, position, rotation));

                        if (Delay != null)
                            yield return new WaitForSeconds(Delay.Value / NumRays);
                    }
                    if (ChangeDirection)
                    {
                        startRotation = clockwise ? midRotaion + MaxRotation / 2 : midRotaion - MaxRotation / 2;
                        clockwise = !clockwise;
                    }
                    if (PatternRepeats != null)
                    {
                        PatternRepeats -= 1;
                        if (PatternRepeats == 0)
                        {
                            break;
                        }
                    }
                } while (PatternRepeats != null);

                if (PulseDelay != null)
                {
                    yield return new WaitForSeconds(PulseDelay.Value);
                    if (ReducePulseDelay != null)
                    {
                        PulseDelay = PulseDelay > MinPulseDelay ? PulseDelay - PulseDelay * ReducePulseDelay : PulseDelay;
                    }
                }

                if (NumRays > MaxRays)
                {
                    NumRays += AddRays;
                }

            } while (Repeat && (Delay != null || PulseDelay != null));
        }
    }
}
