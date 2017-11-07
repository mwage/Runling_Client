using Drones.Movement;
using UnityEngine;

namespace Drones.DroneTypes
{
    public class RedDrone : ADrone
    {
        private readonly Area _area;

        public RedDrone(float speed, float size, DroneColor color, float acceleration, Area area, float waitTime = 0.5f, bool synchronized = false, 
            DroneType droneType = DroneType.FlyingOneWayDrone) :
            base(speed, size, color, droneType, null)
        {
            _area = area;
            MovementType = new PointToPointMovement(area, Size, acceleration, waitTime, synchronized);
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var direction = Random.Range(0, 4);
            var newDrone = Object.Instantiate(factory.SetDroneType[DroneType], DroneStartPosition.GetRandomPosition(Size, _area), Quaternion.Euler(0, -45 + 90 * direction, 0), factory.transform);
            return newDrone;
        }
    }
}