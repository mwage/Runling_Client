using UnityEngine;

namespace Assets.Scripts.Drones
{
    public abstract class ADrone : IDrone
    {
        public float Size { get; private set; }


        protected float Speed;
        protected DroneColor Color;
        protected DroneType DroneType;
        protected DroneMovement.MovementDelegate MoveDelegate;
        protected float? Curving;
        protected float? SinForce;
        protected float? SinFrequency;

        protected ADrone()
        {
        }

        protected ADrone(float speed, float size, DroneColor color, DroneType? droneType = null, DroneMovement.MovementDelegate moveDelegate = null, 
            float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            Speed = speed;
            Size = size;
            Color = color;
            DroneType = droneType ?? DroneType.BouncingDrone;
            MoveDelegate = moveDelegate;
            Curving = curving;
            SinForce = sinForce;
            SinFrequency = sinFrequency;
        }

        public abstract GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null);

        public void ConfigureDrone(GameObject drone)
        {
            // Adjust drone color and size
            var model = drone.transform.GetChild(0);
            foreach (Transform ch in model)
            {
                ch.GetComponent<Renderer>().material = DroneFactory.SetDroneMaterial[Color];
            }

            var scale = drone.transform.localScale;
            scale.x *= Size;
            scale.z *= Size;
            drone.transform.localScale = scale;

            // Move drone
            DroneMovement.Move(drone, Speed, MoveDelegate, Curving, SinForce, SinFrequency);
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
                MoveDelegate = rhs.MoveDelegate;
                Curving = rhs.Curving;
                SinForce = rhs.SinForce;
                SinFrequency = rhs.SinFrequency;
            }
        }
    }

    public enum DroneType
    {
        BouncingDrone,
        FlyingBouncingDrone,
        FlyingOnewayDrone,
        MineDrone,
        MineDroneBouncing,
        MineDroneOneway
    }

    public enum DroneColor
    {
        Grey,
        Blue,
        Red,
        Golden,
        Magenta,
        DarkGreen,
        Cyan
    }
}