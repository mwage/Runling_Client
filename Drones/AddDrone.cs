using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDrone : MonoBehaviour
{
    //attach gameobjects
    public GameObject bouncingDrone;
    public GameObject flyingBouncingDrone;
    public GameObject flyingOnewayDrone;

    //attach scripts
    public MoveDrone _moveDrone;
    public DroneDirection _droneDirection;
    public DroneStartPosition _droneStartPosition;

    // add a basic drone every x seconds with a set speed/size
    public void RandomBouncingDrone(float delay, float droneSpeed, float size, Color color, Area boundary)
    {
        StartCoroutine(AddRandomBouncingDrone(delay, droneSpeed, size, color, boundary));
    }

    // add a bouncing drone every x seconds with a set speed/size at the corner.
    IEnumerator AddRandomBouncingDrone(float delay, float droneSpeed, float size, Color color, Area boundary)
    {
        while (true)
        {
            Renderer rend;
            Vector3 scale;

            yield return new WaitForSeconds(delay);
            
            // spawn new drone within boundaries and with dronespeed
            GameObject newDrone = Instantiate(bouncingDrone, _droneStartPosition.RandomCornerGround(size, boundary), Quaternion.Euler(0, _droneDirection.RandomDirection(1f), 0));

            // adjust drone color and size
            rend = newDrone.GetComponent<Renderer>();
            rend.material.color = color;
            scale = newDrone.transform.localScale;
            scale.x *= size;
            scale.z *= size;
            newDrone.transform.localScale = scale;

            // move drone
            _moveDrone.MoveStraight(newDrone, droneSpeed);
        }
    }

    // add a basic drone every x seconds with a set speed/size
    public void RandomFlyingBouncingDrone(float delay, float droneSpeed, float size, Color color, Area boundary)
    {
        StartCoroutine(AddRandomFlyingBouncingDrone(delay, droneSpeed, size, color, boundary));
    }

    // add a bouncing drone every x seconds with a set speed/size at the corner.
    IEnumerator AddRandomFlyingBouncingDrone(float delay, float droneSpeed, float size, Color color, Area boundary)
    {
        while (true)
        {
            Renderer rend;
            Vector3 scale;

            yield return new WaitForSeconds(delay);

            // spawn new drone within boundaries and with dronespeed
            GameObject newDrone = Instantiate(flyingBouncingDrone, _droneStartPosition.RandomCornerAir(size, boundary), Quaternion.Euler(0, _droneDirection.RandomDirection(1f), 0));

            // adjust drone color and size
            rend = newDrone.GetComponent<Renderer>();
            rend.material.color = color;
            scale = newDrone.transform.localScale;
            scale.x *= size;
            scale.z *= size;
            newDrone.transform.localScale = scale;

            // move drone
            _moveDrone.MoveStraight(newDrone, droneSpeed);

        }
    }

}
