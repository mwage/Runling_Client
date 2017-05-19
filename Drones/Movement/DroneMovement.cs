using System.Collections;
using UnityEngine;

namespace Drones.Movement
{
    public class DroneMovement
    {
        public delegate void MovementDelegate(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null);

        public static void Move(GameObject drone, float droneSpeed, MovementDelegate moveDelegate = null,
            float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            if (moveDelegate == null)
            {
                MoveStraight(drone, droneSpeed);
                var instance = drone.AddComponent<RotateBouncingDrone>();
                instance.DroneSpeed = droneSpeed;
            }
            else
            {
                moveDelegate(drone, droneSpeed, curving, sinForce, sinFrequency);
            }
        }

        //move drones in a straight line
        public static void MoveStraight(GameObject drone, float droneSpeed)
        {
            var rb = drone.GetComponent<Rigidbody>();
            rb.AddForce(drone.transform.forward * droneSpeed, ForceMode.VelocityChange);
        }

        public static void ChaserMovement(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null)
        {
            var instance = drone.AddComponent<ChaserMovement>();
            instance.Speed = droneSpeed;
        }

        public static void CurvedMovement(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<CurvedMovement>();
            instance.Curving = curving ?? 3;
            instance.DroneSpeed = droneSpeed;
            instance.CurvingDuration = 4;
        }

        public static void CircleMovement(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<CurvedMovement>();
            instance.Curving = curving ?? 1;
            instance.DroneSpeed = droneSpeed;
            instance.CurvingDuration = null;
        }

        public static void SinusoidalMovement(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<SinusoidalMovement>();
            instance.SinFrequency = sinFrequency ?? 5;
            instance.SinForce = sinForce ?? 40;
            instance.DroneSpeed = droneSpeed;
        }

        public static void FixedSinusoidalMovement(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null)
        {
            SinusoidalMovement(drone, droneSpeed, sinForce: sinForce, sinFrequency: sinFrequency);
            drone.GetComponent<SinusoidalMovement>().Fixed = true;
        }
    }
}
