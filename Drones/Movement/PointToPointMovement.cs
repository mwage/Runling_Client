using UnityEngine;

namespace Drones.Movement
{
    public class PointToPointMovement : IDroneMovement
    {
        private readonly Area _area;
        private readonly float _size;
        private readonly float _acceleration;
        private readonly float _waitTime;
        private readonly bool _synchronized;

        public PointToPointMovement(Area area, float size, float acceleration, float waitTime = 0.5f, bool synchronized = false)
        {
            _area = area;
            _size = size;
            _acceleration = acceleration;
            _waitTime = waitTime;
            _synchronized = synchronized;
        }

        public void Initialize(GameObject drone, float speed)
        {
            var anim = drone.transform.Find("Model")?.GetComponent<Animator>();
            if (anim != null)
            {
                Object.Destroy(anim);
            }
            drone.AddComponent<PointToPointMovementImplementation>().Initialize(speed, _area, _size, _acceleration, _waitTime, _synchronized);
        }
    }
}
