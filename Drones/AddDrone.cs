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
    public SpawnDrone _spawnDrone;

    // add a basic drone every x seconds with a set speed/size
    public void RandomBouncingDrone(float delay, float droneSpeed, float size, Color color, Area boundary)
    {
        StartCoroutine(IRandomBouncingDrone(delay, droneSpeed, size, color, boundary));
    }

    // add a bouncing drone every x seconds with a set speed/size at the corner.
    IEnumerator IRandomBouncingDrone(float delay, float droneSpeed, float size, Color color, Area boundary)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            Renderer rend;
            Vector3 scale;
                    
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
        StartCoroutine(IRandomFlyingBouncingDrone(delay, droneSpeed, size, color, boundary));
    }

    // add a bouncing drone every x seconds with a set speed/size at the corner.
    IEnumerator IRandomFlyingBouncingDrone(float delay, float droneSpeed, float size, Color color, Area boundary)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            Renderer rend;
            Vector3 scale;
            
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

    public void GridDrones(int droneCount, float delay, float speed, float size, Color color, Area area)
    {
        StartCoroutine (IGridDronesHorizontal(droneCount, delay, speed, size, color, area));
        StartCoroutine(IGridDronesVertical(droneCount, delay, speed, size, color, area));
    }

    IEnumerator IGridDronesHorizontal(int droneCount, float delay, float speed, float size, Color color, Area area)
    {
        float height = area.topBoundary - (0.5f + size / 2);
        float lenght = area.rightBoundary - (0.5f + size / 2);
        float direction = 90f;

        while (true)
        {
            for (int j = 0; j < (int)(lenght / height); j++)
            {
                for (int i = 0; i < droneCount; i++)
                {
                    Vector3 startPos = new Vector3(-lenght, 0.6f, height - i * 2 * height / droneCount);
                    _spawnDrone.StraightFlyingOnewayDrone(speed, size, color, startPos, direction);
                    yield return new WaitForSeconds(delay * 2 * height / droneCount);
                }
                for (int i = 0; i < droneCount; i++)
                {
                    Vector3 startPos = new Vector3(-lenght, 0.6f, -height + i * 2 * height / droneCount);
                    _spawnDrone.StraightFlyingOnewayDrone(speed, size, color, startPos, direction);
                    yield return new WaitForSeconds(delay * 2 * height / droneCount);
                }
            }
            droneCount++;
        }
    }
    IEnumerator IGridDronesVertical(int droneCount, float delay, float speed, float size, Color color, Area area)
    {
        float height = area.topBoundary - (0.5f + size / 2);
        float lenght = area.rightBoundary - (0.5f + size / 2);
        float direction = 180f;
        droneCount *= (int)(lenght/height);

        while (true)
        {
            for (int i = 0; i < droneCount; i++)
            {
                Vector3 startPos = new Vector3(-lenght + i * 2 * lenght / droneCount, 0.6f, height);
                _spawnDrone.StraightFlyingOnewayDrone(speed, size, color, startPos, direction);
                yield return new WaitForSeconds(delay * 2* lenght/ droneCount);
            }
            for (int i = 0; i < droneCount; i++)
            {
                Vector3 startPos = new Vector3(lenght - i * 2 * lenght / droneCount, 0.6f, height);
                _spawnDrone.StraightFlyingOnewayDrone(speed, size, color, startPos, direction);
                yield return new WaitForSeconds(delay * 2 * lenght / droneCount);
            }
            droneCount += (int) (lenght/height);
        }
    }
}
