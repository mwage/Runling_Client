using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2SLA : MonoBehaviour
{

    //attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;



    public void Level2Drones()
    {
        //Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(25, 5f, 1f, Color.blue, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(3f, 5f, 1f, Color.blue, _area.bouncingSLA);
        _spawnDrone.RandomBouncingDrone(15, 4f, 1.5f, Color.red, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(8f, 4f, 1.5f, Color.red, _area.bouncingSLA);
    }
}
