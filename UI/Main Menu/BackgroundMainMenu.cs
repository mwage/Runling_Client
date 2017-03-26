using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundMainMenu : MonoBehaviour
{
    //attach scripts
    public SpawnDrone _spawnDrone;
    public BoundariesSLA _area;


    void Start()
    {
        //Spawn drones (dronecount/delay, speed, size, color, boundaries)
        _spawnDrone.RandomBouncingDrone(20, 3f, 1f, Color.blue, _area.bouncingSLA);
        _spawnDrone.RandomFlyingBouncingDrone(30, 5f, 1f, Color.magenta, _area.flyingSLA);
    }
}

