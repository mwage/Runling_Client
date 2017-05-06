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

        public static void SinusoidalMovement(GameObject drone, float droneSpeed)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<SinusoidalMovement>();
            instance.Frequency = 5;
            instance.SinForce = 20;
        }

        public static void CurvedMovement(GameObject drone, float droneSpeed)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<CurvedMovement>();
            instance.Curving = -7; // deg/s, changes direction of force
            instance.Force  = -10; // acceleration along X axis. Drone doesn't overaccelerate
            instance.DroneSpeed = droneSpeed;
        }

        public static void ChaserMovement(GameObject drone, float droneSpeed, GameObject player)
        {
            var instance = drone.AddComponent<ChaserMovement>();
            instance.Speed = droneSpeed;
            instance.Player = player;
        }
    }
}
