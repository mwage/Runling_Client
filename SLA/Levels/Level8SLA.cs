using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8SLA : MonoBehaviour
{
    // attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;

    public GameObject flyingOnewayDrone;
    public MoveDrone _moveDrone;
    
    public void Level8Drones()
    {
        // Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(10, 6f, 1f, Color.blue, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(4f, 6f, 1f, Color.blue, _area.bouncingSLA);
        _spawnDrone.RandomBouncingDrone(8, 6f, 1.5f, Color.red, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(8f, 6f, 1.5f, Color.red, _area.bouncingSLA);

        // Spawn green drones (initial delay, inbetween delay, size)
        StartCoroutine(GreenDronesLevel8(5f, 1.5f, Color.green));
    }

    IEnumerator GreenDronesLevel8(float delay, float size, Color color)
    {
        Vector3 startPos = new Vector3(0, 0.6f, _area.flyingSLA.bottomBoundary + (0.5f + size / 2));
        Vector3 startPos2 = new Vector3(0, 0.6f, _area.flyingSLA.topBoundary - (0.5f + size / 2));
        int droneCount = 12;
        
        while (true)
        {
            StartCoroutine(IGreenDronesLevel8(droneCount, 7f, size, color, startPos, -90));
            yield return new WaitForSeconds(delay);
            StartCoroutine(IGreenDronesLevel8(droneCount, 7f, size, color, startPos2, 90));
            yield return new WaitForSeconds(delay);

            if (delay > 2f) { delay -= delay * 0.05f; }
            if (droneCount < 32) { droneCount++; }
        }
    }

    IEnumerator IGreenDronesLevel8(int droneCount, float droneSpeed, float size, Color color, Vector3 startPos, float startRotation)
    {
        Renderer rend;
        Vector3 scale;
        float rotation;
        float delay = 2.5f / droneCount;
        Debug.Log(droneCount);
        Debug.Log(delay);

        for (int i = 0; i < droneCount; i++)
        {
            // spawn new drone in set position, direction and dronespeed
            rotation = startRotation + 180 * i / droneCount;
            GameObject newDrone = Instantiate(flyingOnewayDrone, startPos, Quaternion.Euler(0, rotation, 0));

            // adjust drone color and size
            rend = newDrone.GetComponent<Renderer>();
            rend.material.color = color;
            scale = newDrone.transform.localScale;
            scale.x *= size;
            scale.z *= size;
            newDrone.transform.localScale = scale;

            // move drone
            _moveDrone.MoveStraight(newDrone, droneSpeed);
            yield return new WaitForSeconds(delay);
        }

        for (int i = 0; i < droneCount; i++)
        {
            // spawn new drone in set position, direction and dronespeed
            rotation = startRotation + 180 - 180 * i / droneCount;
            GameObject newDrone = Instantiate(flyingOnewayDrone, startPos, Quaternion.Euler(0, rotation, 0));

            // adjust drone color and size
            rend = newDrone.GetComponent<Renderer>();
            rend.material.color = color;
            scale = newDrone.transform.localScale;
            scale.x *= size;
            scale.z *= size;
            newDrone.transform.localScale = scale;

            // move drone
            _moveDrone.MoveStraight(newDrone, droneSpeed);
            yield return new WaitForSeconds(delay);
        }
    }
}