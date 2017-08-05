using MP.TSGame.Drones.Movement;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Types
{
    public class RandomDrone : ADrone
    {
        protected FP RestrictedZone;
        protected float? ConeRange;
        protected FP StartDirection;

        public RandomDrone(IDrone sourceDrone, FP? restrictedZone = null, float? coneRange = null, FP? startDirection = null)
        {
            CopyFrom(sourceDrone);
            RestrictedZone = restrictedZone ?? 1;
            ConeRange = coneRange;
            StartDirection = startDirection ?? 0;
        }

        public RandomDrone(FP speed, FP size, DroneColor color, DroneType? droneType = null, FP? restrictedZone = null, float? coneRange = null, FP? startDirection = null,
            DroneMovement.MovementDelegate moveDelegate = null, FP? curving = null, FP? sinForce = null, FP? sinFrequency = null) : 
            base(speed, size, color, droneType, moveDelegate, curving, sinForce, sinFrequency)
        {
            RestrictedZone = restrictedZone ?? 1;
            ConeRange = coneRange;
            StartDirection = startDirection ?? 0;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            TSVector pos;
            if (posDelegate != null)
                pos = posDelegate((float)Size, area);
            else
            {
                pos = isAdded
                    ? DroneStartPosition.GetRandomCorner((float)Size, area)
                    : DroneStartPosition.GetRandomPosition((float)Size, area);
            }

            var newDrone = TrueSyncManager.SyncedInstantiate(factory.SetDroneType[DroneType], pos,
                TSQuaternion.Euler(0, StartDirection + DroneDirection.RandomDirection(RestrictedZone, ConeRange), 0));
                newDrone.transform.SetParent(factory.transform);
            return newDrone;
        }
    }
}
