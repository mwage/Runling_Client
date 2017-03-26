using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1SLA : MonoBehaviour {

    //attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;

    
    public void Level1Drones()
    {
        //Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(60, 4f, 1f, Color.blue, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(1.5f, 4f, 1f, Color.blue, _area.bouncingSLA);
    }
}
