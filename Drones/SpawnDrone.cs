using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDrone : MonoBehaviour
{
    //attach gameobjects
    public GameObject bouncingDrone;
    public GameObject flyingBouncingDrone;
    public GameObject flyingOnewayDrone;

    //attach scripts
    public MoveDrone _moveDrone;
    public DroneDirection _droneDirection;
    public DroneStartPosition _droneStartPosition;

    // create x basic drones bouncing off walls with set speed and size
    public void RandomBouncingDrone(int droneCount, float droneSpeed, float size, Color color, Area boundary)
    {
        Renderer rend;
        Vector3 scale;
        
        for (int i = 0; i < droneCount; i++)
        {
            // spawn new drone within boundaries and with dronespeed
            GameObject newDrone = Instantiate(bouncingDrone, _droneStartPosition.RandomPositionGround(size, boundary), Quaternion.Euler(0,_droneDirection.RandomDirection(1f), 0));
 
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

    public void RandomFlyingBouncingDrone(int droneCount, float droneSpeed, float size, Color color, Area boundary)
    {
        Renderer rend;
        Vector3 scale;

        for (int i = 0; i < droneCount; i++)
        {
            // spawn new drone within boundaries and with dronespeed
            GameObject newDrone = Instantiate(flyingBouncingDrone, _droneStartPosition.RandomPositionAir(size, boundary), Quaternion.Euler(0, _droneDirection.RandomDirection(1f), 0));

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

    public void StraightFlyingOnewayDrone(float droneSpeed, float size, Color color, Vector3 startPos, float direction)
    {
        Renderer rend;
        Vector3 scale;


            // spawn new drone in set position, direction and dronespeed
            GameObject newDrone = Instantiate(flyingOnewayDrone, startPos, Quaternion.Euler(0, direction, 0));

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

    public void StraightFlying360Drones(int droneCount, float droneSpeed, float size, Color color, Vector3 startPos)
    {
        Renderer rend;
        Vector3 scale;

        for (int i = 1; i <= droneCount; i++)
        {
            // spawn new drone in set position, direction and dronespeed
            GameObject newDrone = Instantiate(flyingOnewayDrone, startPos, Quaternion.Euler(0, 360*i/droneCount, 0));

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