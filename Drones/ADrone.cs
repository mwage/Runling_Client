using UnityEngine;

namespace Assets.Scripts.Drones
{
    public abstract class ADrone : IDrone
    {
        protected float Speed;
        protected float Size;
        protected Color Color;

        protected ADrone(float speed, float size, Color color)
        {
            Speed = speed;
            Size = size;
            Color = color;
        }

        public abstract GameObject CreateDroneInstance(DroneFactory factory, bool isAdded);

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
            MoveDrone.MoveStraight(drone, Speed);
        }
    }
}
