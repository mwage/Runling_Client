using UnityEngine;
using Assets.Scripts.Drones;


namespace Assets.Scripts.Drones
{ 
    public class MoveDrone
    {

        //move drones in a straight line
        public static void MoveStraight(GameObject drone, float droneSpeed)
        {
            //apply dronespeed
            var rb = drone.GetComponent<Rigidbody>();
            rb.AddForce(drone.transform.forward * droneSpeed, ForceMode.VelocityChange);
        }

        public static void Sinusoidal(GameObject drone)
        {
            drone.AddComponent<MoveSinusoidal>();
        }
    }
}
