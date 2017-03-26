using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4SLA : MonoBehaviour
{

    // attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;



    public void Level4Drones()
    {
        // Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(20, 5f, 1f, Color.blue, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(4f, 5f, 1f, Color.blue, _area.bouncingSLA);
        _spawnDrone.RandomBouncingDrone(15, 4f, 1.5f, Color.red, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(10f, 4f, 1.5f, Color.red, _area.bouncingSLA);

        // Spwn green drones
        StartCoroutine(GreenDronesLevel4(5f, 1.5f));
    }

    IEnumerator GreenDronesLevel4(float delay, float size)
    {
        int droneCount = 0;
        while (true)
        {
            Vector3 startPos = new Vector3();
            Vector3 startPos2 = new Vector3();
            startPos = Location(2f, _area.flyingSLA);
            startPos2 = OtherLocation(2f, _area.flyingSLA);

            _spawnDrone.StraightFlying360Drones(16 + 4*droneCount, 10f, size, Color.green, startPos);
            yield return new WaitForSeconds(delay);
            _spawnDrone.StraightFlying360Drones(16 + 4*droneCount+2, 10f, size, Color.green, startPos2);
            yield return new WaitForSeconds(delay);
            if (delay > 0.6f) { delay -= 0.1f; };
            droneCount++;
        }
    }

    public Vector3 Location(float size, Area boundary)
    {
        Vector3 startPos = new Vector3();
        int location = Random.Range(0, 6);

        if (location == 0)
        {
            startPos.Set(boundary.leftBoundary + (10.5f + size / 2), 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
        }
        else if (location == 1)
        {
            startPos.Set(boundary.rightBoundary - (10.5f + size / 2), 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
        }
        else if (location == 2)
        {
            startPos.Set(boundary.leftBoundary + (20.5f + size / 2), 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
        }
        else if (location == 3)
        {
            startPos.Set(boundary.rightBoundary - (20.5f + size / 2), 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
        }
        else if (location == 4)
        {
            startPos.Set(boundary.leftBoundary + (30.5f + size / 2), 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
        }
        else if (location == 5)
        {
            startPos.Set(boundary.rightBoundary - (0.5f + size / 2), 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
        }

        return startPos;
    }

    public Vector3 OtherLocation(float size, Area boundary)
    {
        Vector3 startPos = new Vector3();
        int location = Random.Range(0, 6);

        if (location == 0)
        {
            startPos.Set(boundary.leftBoundary + (10.5f + size / 2), 0.6f, boundary.topBoundary - (0.5f + size / 2));
        }
        else if (location == 1)
        {
            startPos.Set(boundary.rightBoundary - (10.5f + size / 2), 0.6f, boundary.topBoundary - (0.5f + size / 2));
        }
        else if (location == 2)
        {
            startPos.Set(boundary.leftBoundary + (20.5f + size / 2), 0.6f, boundary.topBoundary - (0.5f + size / 2));
        }
        else if (location == 3)
        {
            startPos.Set(boundary.rightBoundary - (20.5f + size / 2), 0.6f, boundary.topBoundary - (0.5f + size / 2));
        }
        else if (location == 4)
        {
            startPos.Set(boundary.leftBoundary + (30.5f + size / 2), 0.6f, boundary.topBoundary - (0.5f + size / 2));
        }
        else if (location == 5)
        {
            startPos.Set(boundary.rightBoundary - (30.5f + size / 2), 0.6f, boundary.topBoundary - (0.5f + size / 2));
        }
        return startPos;
    }
}
