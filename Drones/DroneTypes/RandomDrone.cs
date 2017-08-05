using System.IO;
using Drones.Movement;
using UnityEngine;

namespace Drones.DroneTypes
{
    public class RandomDrone : ADrone
    {
        protected float RestrictedZone;
        protected float? ConeRange;
        protected float StartDirection;

        public RandomDrone(IDrone sourceDrone, float? restrictedZone = null, float? coneRange = null, float? startDirection = null)
        {
            CopyFrom(sourceDrone);
            RestrictedZone = restrictedZone ?? 1;
            ConeRange = coneRange;
            StartDirection = startDirection ?? 0;
        }

        public RandomDrone(float speed, float size, DroneColor color, DroneType? droneType = null, float? restrictedZone = null, float? coneRange = null, float? startDirection = null,
            DroneMovement.MovementDelegate moveDelegate = null, float? curving = null, float? sinForce = null, float? sinFrequency = null) : 
            base(speed, size, color, droneType, moveDelegate, curving, sinForce, sinFrequency)
        {
            RestrictedZone = restrictedZone ?? 1;
            ConeRange = coneRange;
            StartDirection = startDirection ?? 0;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            Vector3 pos;
            if (posDelegate != null)
                pos = posDelegate(Size, area);
            else
            {
                pos = isAdded
                    ? DroneStartPosition.GetRandomCorner(Size, area)
                    : DroneStartPosition.GetRandomPosition(Size, area);
            }

            var newDrone = Object.Instantiate(factory.SetDroneType[DroneType], pos, Quaternion.Euler(0, StartDirection + DroneDirection.RandomDirection(RestrictedZone, ConeRange), 0));
            return newDrone;
        }
    }
}
