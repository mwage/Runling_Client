using Drones.DroneTypes;
using UnityEngine;

namespace Drones.Movement
{
    public class CurvedMovement : IDroneMovement
    {
        private readonly float _curving;
        private readonly float? _curvingDuration;

        public CurvedMovement(float curving, float? curvingDuration = 4)
        {
            _curving = curving;
            _curvingDuration = curvingDuration;
        }

        public void Initialize(GameObject drone, float speed)
        {
            var anim = drone.transform.Find("Model")?.GetComponent<Animator>();
            anim?.SetFloat(ADrone.SpeedHash, 3 + speed / 2);
            drone.AddComponent<CurvedMovementImplementation>().Initialize(speed, _curving, _curvingDuration);
        }
    }
}
