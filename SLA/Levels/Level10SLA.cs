using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level10SLA : MonoBehaviour
{

    //attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;


    public void Level10Drones()
    {
        //Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(10, 7f, 1f, Color.blue, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(4f, 7f, 1f, Color.blue, _area.bouncingSLA);

        // Grid Drones
        _addDrone.GridDrones(10, 0.05f, 7f, 1f, Color.magenta, _area.flyingSLA);
    }
}
