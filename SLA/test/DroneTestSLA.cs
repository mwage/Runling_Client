using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTestSLA : MonoBehaviour
{

    // attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;


    public void LoadDrones()
    {
        // Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(5, 5f, 1f, Color.grey, _area.test);
        _spawnDrone.RandomBouncingDrone(5, 5f, 2f, Color.grey, _area.test);

    }
}