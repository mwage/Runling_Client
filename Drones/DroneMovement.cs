using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class DroneMovement
    {
        public delegate void MovementDelegate(GameObject drone, float droneSpeed);

        public static void Move(GameObject drone, float droneSpeed, MovementDelegate moveDelegate = null)
        {
            if (moveDelegate == null)
            {
                MoveStraight(drone, droneSpeed);
            }
            else
            {
                moveDelegate(drone, droneSpeed);
            }
        }

        //move drones in a straight line
        public static void MoveStraight(GameObject drone, float droneSpeed)
        {
            var rb = drone.GetComponent<Rigidbody>();
            rb.AddForce(drone.transform.forward * droneSpeed, ForceMode.VelocityChange);
        }

        public static void DoubleSpeed(GameObject drone, float droneSpeed)
        {
            var rb = drone.GetComponent<Rigidbody>();
            rb.AddForce(drone.transform.forward * 2 * droneSpeed, ForceMode.VelocityChange);
        }

        public static void HalfSpeed(GameObject drone, float droneSpeed)
        {
            var rb = drone.GetComponent<Rigidbody>();
            rb.AddForce(drone.transform.forward / 2 * droneSpeed, ForceMode.VelocityChange);
        }
    }
}
