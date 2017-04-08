using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class StraightFlying360Drone : ADrone
    {
        protected readonly int NumRays;
        protected readonly float? Delay;
        
        public StraightFlying360Drone(float speed, float size, Color color, int numRays, float? delay = null) : base(speed, size, color)
        {
            NumRays = numRays;
            Delay = delay;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            factory.StartCoroutine(Generate360Drones(factory, area, posDelegate));
            return null;
        }

        private IEnumerator Generate360Drones(DroneFactory factory, Area area, StartPositionDelegate posDelegate)
        {
            var clockwise = true;
            var startRotation = 0f;

            var position = posDelegate(Size, area);
            
            // If delay is not null, the drones will go out in a fan motion.  If it is null, all rays will go out at the same time
            if (Delay != null)
            {
                /////////// Note:  Change this so we calculate clockwise and startRotation based on position
                var isTop = posDelegate == DroneStartPosition.GetRandomTopSector;
                clockwise = (isTop) ? position.x < 0 : position.x >= 0;
                startRotation = (position.x < 0) ? 90f : -90f;
                
            }

            for (var i = 0; i < NumRays; i++)
            {
                // spawn new drone in set position, direction and dronespeed
                var rotation = startRotation + (clockwise ? 1 : -1) * (360f * i / NumRays);
                var drone = Object.Instantiate(factory.FlyingOnewayDrone, position, Quaternion.Euler(0, rotation, 0));
                ConfigureDrone(drone);

                if (Delay != null)
                    yield return new WaitForSeconds(Delay.Value);
            }
        }
    }
}
