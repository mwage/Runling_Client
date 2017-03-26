using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5SLA : MonoBehaviour
{

    //attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;



    public void Level5Drones()
    {
         // Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(20, 4f, 1.5f, Color.red, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(6f, 4f, 1.5f, Color.red, _area.bouncingSLA);
        _spawnDrone.RandomFlyingBouncingDrone(100, 6f, 1f, Color.magenta, _area.flyingSLA);
        _addDrone.RandomFlyingBouncingDrone(0.8f, 6f, 1f, Color.magenta, _area.flyingSLA);
    }
}
