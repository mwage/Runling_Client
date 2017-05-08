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

        public static void SinusoidalMovement(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<SinusoidalMovement>();
            instance.SinFrequency = sinFrequency ?? 5;
            instance.SinForce = sinForce ?? 20;
            instance.DroneSpeed = droneSpeed;
        }

        public static void CurvedMovement(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<CurvedMovement>();
            instance.Curving  = curving ?? 0.8f;
            instance.DroneSpeed = droneSpeed;
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

        public static void CurvedSinudoidalMovement(GameObject drone, float droneSpeed, GameObject player = null, float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            MoveStraight(drone, droneSpeed);
            var sinInstance = drone.AddComponent<SinusoidalMovement>();
            var curvedInstance = drone.AddComponent<CurvedMovement>();
            sinInstance.SinFrequency = sinFrequency ?? 5;
            sinInstance.SinForce = sinForce ?? 20;
            curvedInstance.Curving = curving ?? 1;
            curvedInstance.DroneSpeed = droneSpeed;
        }
    }
}
