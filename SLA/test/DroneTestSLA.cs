using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTestSLA : MonoBehaviour {

    public GameObject bouncingDrone;

    //attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;



    public void LoadDrones()
    {
        //Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomFlyingBouncingDrone(80, 5f, 1f, Color.magenta, _area.flyingSLA);
        _addDrone.RandomFlyingBouncingDrone(0.2f, 5f, 1f, Color.magenta, _area.flyingSLA);
    }


}
