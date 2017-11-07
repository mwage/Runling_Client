using Drones.Movement;
using UnityEngine;

namespace Drones.DroneTypes
{
    public abstract class ADrone : IDrone
    {
        public float Size { get; private set; }
        protected float Speed { get; set; }
        protected DroneColor Color { get; set; }
        protected DroneType DroneType { get; set; }
        protected IDroneMovement MovementType { get; set; }
        public static int SpeedHash => Animator.StringToHash("DroneSpeed");

        protected ADrone()
        {
        }

        protected ADrone(float speed, float size, DroneColor color, DroneType droneType, IDroneMovement movementType)
        {
            Speed = speed;
            Size = size;
            Color = color;
            DroneType = droneType;
            MovementType = movementType ?? new StraightMovement();
        }

        public abstract GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null);

        public void ConfigureDrone(GameObject drone, DroneFactory factory)
        {
            var model = drone.transform.Find("Model");

            foreach (Transform child in model)
            {
                if (child.name == "Top") continue;
                if (child.name == "Sphere")
                {
                    foreach (Transform ch in child)
                    {
                        ch.GetComponent<Renderer>().material = factory.SetDroneMaterial[Color];
                    }
                }
                child.GetComponent<Renderer>().material = factory.SetDroneMaterial[Color];
            }

            drone.transform.localScale = Size * Vector3.one;

            if (DroneType == DroneType.BouncingDrone || DroneType == DroneType.FlyingBouncingDrone ||
                DroneType == DroneType.FlyingOneWayDrone)
            {
                if (Size > 1)
                {
                    model.transform.localPosition += new Vector3(0, (Size - 1) / 7, 0);
                }
            }

            // Set up Drone Movement
            MovementType.Initialize(drone, Speed);
        }

        protected void CopyFrom(IDrone sourceDrone)
        {
            var rhs = sourceDrone as ADrone;
            if (rhs != null)
            {
                Speed = rhs.Speed;
                Size = rhs.Size;
                Color = rhs.Color;
                DroneType = rhs.DroneType;
                MovementType = rhs.MovementType;
            }
        }
    }

    public enum DroneType
    {
        BouncingDrone,
        FlyingBouncingDrone,
        FlyingOneWayDrone,
        FlyingBouncingMine,
        BouncingMine,
        FlyingOneWayMine
    }

    public enum DroneColor
    {
        Grey,
        Blue,
        Red,
        Golden,
        Magenta,
        DarkGreen,
        Cyan, 
        BrightGreen
    }
}