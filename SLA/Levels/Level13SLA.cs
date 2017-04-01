using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level13SLA : MonoBehaviour
{

    //attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;


    public void Level13Drones()
    {
        //Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(8, 12f, 1f, Color.blue, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(6f, 12f, 1f, Color.blue, _area.bouncingSLA);
        _spawnDrone.RandomBouncingDrone(8, 8f, 1.15f, Color.magenta, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(7f, 8f, 1.15f, Color.magenta, _area.bouncingSLA);
        _spawnDrone.RandomBouncingDrone(8, 4f, 1.5f, Color.red, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(8f, 4f, 1.5f, Color.red, _area.bouncingSLA);
    }
}