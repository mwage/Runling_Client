using UnityEngine;

namespace Assets.Scripts.Drones
{
    public abstract class ADrone : IDrone
    {
        public float Size { get; private set; }


        protected float Speed;
        protected Color Color;
        protected DroneType DroneType;
        protected DroneMovement.MovementDelegate MoveDelegate;
        protected GameObject Player;
        protected float? Curving;
        protected float? SinForce;
        protected float? SinFrequency;

        protected ADrone()
        {
        }

        protected ADrone(float speed, float size, Color color, DroneType? droneType = null, DroneMovement.MovementDelegate moveDelegate = null,
            GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            Speed = speed;
            Size = size;
            Color = color;
            DroneType = droneType ?? DroneType.BouncingDrone;
            MoveDelegate = moveDelegate;
            Player = player;
            Curving = curving;
            SinForce = sinForce;
            SinFrequency = sinFrequency;
        }

        public abstract GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null);

        public void ConfigureDrone(GameObject drone)
        {
            // Adjust drone color and size
            var rend = drone.GetComponent<Renderer>();
            rend.material.color = Color;
            var scale = drone.transform.localScale;
            scale.x *= Size;
            scale.z *= Size;
            drone.transform.localScale = scale;

            // Move drone
            DroneMovement.Move(drone, Speed, MoveDelegate, Player, Curving, SinForce, SinFrequency);
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
                Player = rhs.Player;
                Curving = rhs.Curving;
                SinForce = rhs.SinForce;
                SinFrequency = rhs.SinFrequency;
            }
        }
    }
}