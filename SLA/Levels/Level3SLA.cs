using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3SLA : MonoBehaviour
{

    //attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;



    public void Level3Drones()
    {
        // Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomFlyingBouncingDrone(130, 5f, 1f, Color.magenta, _area.flyingSLA);
        _addDrone.RandomFlyingBouncingDrone(0.5f, 5f, 1f, Color.magenta, _area.flyingSLA);
    }
}
