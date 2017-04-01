using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level12SLA : MonoBehaviour
{

    // attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;

    public void Level12Drones()
    {
        // Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(15, 7f, 1.5f, Color.red, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(7f, 7f, 1.5f, Color.red, _area.bouncingSLA);

        // Spawn green drones (initial delay, size)
        StartCoroutine(GreenDronesLevel12(4f, 8f, 1.5f));
    }

    IEnumerator GreenDronesLevel12(float delay, float speed, float size)
    {
        while (true)
        {
            Vector3 startPos = new Vector3();
            Vector3 startPos2 = new Vector3();
            float rotation;
            float rotation2;
            bool clockwise;
            bool clockwise2;
            startPos = Location(size, _area.flyingSLA);
            startPos2 = OtherLocation(size, _area.flyingSLA);
            int droneCount = 0;

            if (startPos.x < 0)
            {
                rotation = 90f;
                clockwise = false;
            }
            else
            {
                rotation = -90f;
                clockwise = true;
            }

            if (startPos2.x < 0)
            {
                rotation2 = 90f;
                clockwise2 = true;
            }
            else
            {
                rotation2 = -90f;
                clockwise2 = false;
            }

            _spawnDrone.DelayedStraightFlying360Drones(24+droneCount, 0.2f, speed, size, Color.green, startPos, rotation, clockwise);
            _spawnDrone.DelayedStraightFlying360Drones(24+droneCount, 0.2f, speed, size, Color.green, startPos2, rotation2, clockwise2);
            yield return new WaitForSeconds(delay);
            if (delay > 1.5f) { delay -= delay * 0.03f; }
            if (droneCount < 24) { droneCount += 2; }
        }
    }

    public Vector3 Location(float size, Area boundary)
    {
        Vector3 startPos = new Vector3();
        int location = Random.Range(0, 7);

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
            startPos.Set(boundary.rightBoundary - (30.5f + size / 2), 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
        }
        else if (location == 6)
        {
            startPos.Set(0, 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
        }

        return startPos;
    }

    public Vector3 OtherLocation(float size, Area boundary)
    {
        Vector3 startPos = new Vector3();
        int location = Random.Range(0, 7);

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
        else if (location == 6)
        {
            startPos.Set(0, 0.6f, boundary.topBoundary - (0.5f + size / 2));
        }

        return startPos;
    }
}
