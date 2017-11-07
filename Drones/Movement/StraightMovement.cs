using Drones.DroneTypes;
using UnityEngine;

namespace Drones.Movement
{
    public class StraightMovement : IDroneMovement
    {
        public StraightMovement()
        {
        }

        public void Initialize(GameObject drone, float speed)
        {
            var anim = drone.transform.Find("Model")?.GetComponent<Animator>();
            anim?.SetFloat(ADrone.SpeedHash, 3 + speed / 2);

            drone.AddComponent<StraightMovementImplementation>().Initialize(speed);
        }
    }
}
