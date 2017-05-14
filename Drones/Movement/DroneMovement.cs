using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class DroneMovement
    {
        public delegate void MovementDelegate(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null);

        public static void Move(GameObject drone, float droneSpeed, MovementDelegate moveDelegate = null, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            if (moveDelegate == null)
            {
                MoveStraight(drone, droneSpeed);
            }
            else
            {
                moveDelegate(drone, droneSpeed, player, curving, sinForce, sinFrequency);
            }
        }


        //move drones in a straight line
        public static void MoveStraight(GameObject drone, float droneSpeed)
        {
            var rb = drone.GetComponent<Rigidbody>();
            rb.AddForce(drone.transform.forward * droneSpeed, ForceMode.VelocityChange);
        }

        public static void ChaserMovement(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            if (player == null)
            {
                Debug.Log("No player set! Canceled ChaserMovement.");
                return;
            }
            var instance = drone.AddComponent<ChaserMovement>();
            instance.Speed = droneSpeed;
            instance.Player = player;
        }

        public static void CurvedMovement(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<CurvedMovement>();
            instance.Curving = curving ?? 1;
            instance.DroneSpeed = droneSpeed;
        }

        public static void SinusoidalMovement(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<SinusoidalMovement>();
            instance.SinFrequency = sinFrequency ?? 5;
            instance.SinForce = sinForce ?? 40;
            instance.DroneSpeed = droneSpeed;
        }

        public static void FixedSinusoidalMovement(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            SinusoidalMovement(drone, droneSpeed, sinForce: sinForce, sinFrequency: sinFrequency);
            drone.GetComponent<SinusoidalMovement>().Fixed = true;
        }

        public static void CosinusoidalMovement(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            SinusoidalMovement(drone, droneSpeed, sinForce: sinForce, sinFrequency: sinFrequency);
            drone.GetComponent<SinusoidalMovement>().Offset = -Mathf.PI/2;
        }

        public static void FixedCosinusoidalMovement(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            CosinusoidalMovement(drone, droneSpeed, sinForce: sinForce, sinFrequency: sinFrequency);
            drone.GetComponent<SinusoidalMovement>().Fixed = true;
        }

        public static void CurvedSinudoidalMovement(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            SinusoidalMovement(drone, droneSpeed, sinForce: sinForce, sinFrequency: sinFrequency);
            var curvedInstance = drone.AddComponent<CurvedMovement>();
            curvedInstance.Curving = curving ?? 1;
            curvedInstance.DroneSpeed = droneSpeed;
        }
    }
}
