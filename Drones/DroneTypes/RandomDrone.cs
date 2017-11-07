using Drones.Movement;
using Players;
using UnityEngine;

namespace Drones.DroneTypes
{
    public class RandomDrone : ADrone
    {
        private readonly float _restrictedZone;
        private readonly float _coneRange;
        private readonly float _startDirection;

        public RandomDrone(IDrone sourceDrone, float restrictedZone = 1, float coneRange = 360, float startDirection = 0)
        {
            CopyFrom(sourceDrone);
            _restrictedZone = restrictedZone;
            _coneRange = coneRange;
            _startDirection = startDirection;
        }

        public RandomDrone(float speed, float size, DroneColor color, DroneType droneType = DroneType.BouncingDrone, float restrictedZone = 1, float coneRange = 360, float startDirection = 0,
            IDroneMovement movementType = null) : base(speed, size, color, droneType, movementType)
        {
            _restrictedZone = restrictedZone;
            _coneRange = coneRange;
            _startDirection = startDirection;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            Vector3 pos;
            if (posDelegate != null)
            {
                pos = posDelegate(Size, area);
            }
            else
            {
                pos = isAdded
                    ? DroneStartPosition.GetRandomCorner(Size, area)
                    : DroneStartPosition.GetRandomPosition(Size, area);
            }

            var newDrone = Object.Instantiate(factory.SetDroneType[DroneType], pos, 
                Quaternion.Euler(0, _startDirection + DroneDirection.RandomDirection(_restrictedZone, _coneRange), 0), factory.transform);
            return newDrone;
        }
    }
}
