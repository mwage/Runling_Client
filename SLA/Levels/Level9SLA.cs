using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level9SLA : MonoBehaviour
{
    // attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;
    public GameObject flyingOnewayDrone;
    public Chaser _chaser;
    public InitializeGameSLA _initializeGameSLA;
    

    public void Level9Drones()
    {
        // Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(15, 7f, 1.5f, Color.red, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(9f, 7f, 1.5f, Color.red, _area.bouncingSLA);

        // Spawn chaser
        GameObject chaser = Instantiate(flyingOnewayDrone, new Vector3(0, 0.6f, 0), Quaternion.identity);
        GameObject player = _initializeGameSLA.newPlayer;
        _chaser.ChaserDrone(chaser, player, 9f, 1.2f, Color.yellow);

        // Spawn green drones (initial delay, size)
        StartCoroutine(GreenDronesLevel9(4f, 1.5f));
    }

    IEnumerator GreenDronesLevel9(float delay, float size)
    {
        int droneCount = 0;
        while (true)
        {
            Vector3 startPos = new Vector3();
            Vector3 startPos2 = new Vector3();
            startPos = Location(size, _area.flyingSLA);
            startPos2 = OtherLocation(size, _area.flyingSLA);

            _spawnDrone.StraightFlying360Drones(16 + 2 * droneCount, 8f, size, Color.green, startPos);
            yield return new WaitForSeconds(delay);
            _spawnDrone.StraightFlying360Drones(16 + 2 * droneCount, 8f, size, Color.green, startPos2);
            yield return new WaitForSeconds(delay);
            if (delay > 1f) { delay -= delay * 0.1f; }
            if (droneCount < 15) { droneCount++; }
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
            startPos.Set(boundary.rightBoundary - (0.5f + size / 2), 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
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
