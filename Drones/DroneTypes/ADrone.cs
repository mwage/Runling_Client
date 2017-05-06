using UnityEngine;

namespace Assets.Scripts.Drones
{
    public abstract class ADrone : IDrone
    {
        protected float Speed;
        protected float Size;
        protected Color Color;
        protected DroneType DroneType;

        protected ADrone(float speed, float size, Color color, DroneType? droneType = null)
        {
            Speed = speed;
            Size = size;
            Color = color;
            DroneType = droneType ?? DroneType.BouncingDrone;
        }

        public abstract GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null);

        public void ConfigureDrone(GameObject drone, DroneMovement.MovementDelegate moveDelegate = null)
        {
            // Adjust drone color and size
            var rend = drone.GetComponent<Renderer>();
            rend.material.color = Color;
            var scale = drone.transform.localScale;
            scale.x *= Size;
            scale.z *= Size;
            drone.transform.localScale = scale;

            // Move drone
            DroneMovement.Move(drone, Speed, moveDelegate);
        }

        public float GetSpeed()
        {
            return Speed;
        }

        public float GetSize()
        {
            return Size;
        }

        public Color GetColor()
        {
            return Color;
        }

        public DroneType GetDroneType()
        {
            return DroneType;
        }
    }
}