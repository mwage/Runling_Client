using Game.Scripts.Drones.DroneTypes;
using UnityEngine;

namespace Game.Scripts.Drones.Movement
{
    public class StraightMovement : IDroneMovement
    {
        public void Initialize(GameObject drone, float speed)
        {
            var anim = drone.transform.Find("Model")?.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetFloat(ADrone.SpeedHash, 3 + speed / 2);
            }

            drone.AddComponent<StraightMovementImplementation>().Initialize(speed);
        }
    }
}
