using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDrone : MonoBehaviour
{

    //move drones in a straight line
    public void MoveStraight(GameObject drone, float droneSpeed)
    {
        //apply dronespeed
        Rigidbody rb = drone.GetComponent<Rigidbody>();
        rb.AddForce(drone.transform.forward * droneSpeed, ForceMode.VelocityChange);
    }
}
