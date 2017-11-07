using UnityEngine;

namespace Drones.Movement
{
    public class SinusoidalMovement : IDroneMovement
    {
        private readonly float _sinForce;
        private readonly float _sinFrequency;
        private readonly bool _fixedSpeed;

        public SinusoidalMovement(float sinForce, float sinFrequency, bool fixedSpeed = true)
        {
            _sinForce = sinForce;
            _sinFrequency = sinFrequency;
            _fixedSpeed = fixedSpeed;
        }

        public void Initialize(GameObject drone, float speed)
        {
            var anim = drone.transform.Find("Model")?.GetComponent<Animator>();
            if (anim != null)
            {
                Object.Destroy(anim);
            }
            drone.AddComponent<SinusoidalMovementImplementation>().Initialize(speed, _sinForce, _sinFrequency, _fixedSpeed);
        }
    }
}
