using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level7SLA : MonoBehaviour
{

    // attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;
    public MoveDrone _moveDrone;
    public GameObject flyingOnewayDrone;

    GameObject mine;



    public void Level7Drones()
    {

        // Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(15, 5f, 1.5f, Color.red, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(6f, 5f, 1.5f, Color.red, _area.bouncingSLA);

        // Spawn Mine (speed, size)
        mine = _spawnDrone.MineSLA(4f, 3f, _area.flyingSLA);

        // Spawn Green drones (dronecount, delay, speed, size, color)
        StartCoroutine(GreenDronesLevel7(64, 0.1f, 7f, 1.5f, Color.cyan));
    }

    IEnumerator GreenDronesLevel7(int droneCount, float delay, float droneSpeed, float size, Color color)
    {
        while (true)
        {
            Renderer rend;
            Vector3 scale;

            for (int i = 0; i < droneCount; i++)
            {
                // spawn new drone in set position, direction and dronespeed
                GameObject newDrone = Instantiate(flyingOnewayDrone, mine.transform.position, Quaternion.Euler(0, i * 720 / droneCount, 0));

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
                GameObject newDrone = Instantiate(flyingOnewayDrone, mine.transform.position, Quaternion.Euler(0, -i * 720 / droneCount, 0));

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
}