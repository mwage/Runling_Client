using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class RedDrone : ADrone
    {
        protected readonly float Acceleration;
        protected readonly Area Area;
        protected readonly float WaitTime;
        protected readonly bool Synchronized;

        public RedDrone(float speed, float size, DroneColor color, float acceleration, Area area, float? waitTime = null, bool? synchronized = null, DroneType? droneType = null) :
            base(speed, size, color, droneType)
        {
            DroneType = droneType ?? DroneType.MineDrone;
            Acceleration = acceleration;
            Area = area;
            Synchronized = synchronized ?? false;
            WaitTime = waitTime ?? 0.5f;
            MoveDelegate = delegate {};
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var direction = Random.Range(0, 4);
            var newDrone = Object.Instantiate(DroneFactory.SetDroneType[DroneType], DroneStartPosition.GetRandomPositionAir(Size, Area), Quaternion.Euler(0, -45 + 90 * direction, 0));
            var instance = newDrone.AddComponent<PointToPointMovement>();
            instance.Acceleration = Acceleration;
            instance.MaxVelocity = Speed;
            instance.Area = Area;
            instance.Size = Size;
            instance.WaitTime = WaitTime;
            instance.Synchronized = Synchronized;

            return newDrone;
        }
    }
}