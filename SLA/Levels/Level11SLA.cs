using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level11SLA : MonoBehaviour
{

    // attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;

    GameObject mine;
    GameObject mine2;
    GameObject mine3;


    public void Level11Drones()
    {

        // Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(10, 7f, 1.5f, Color.red, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(7f, 7f, 1.5f, Color.red, _area.bouncingSLA);

        // Spawn Mine (speed, size)
        mine = _spawnDrone.MineSLA(5f, 3f, _area.flyingSLA);
        mine2 = _spawnDrone.MineSLA(5f, 3f, _area.flyingSLA);
        mine3 = _spawnDrone.MineSLA(5f, 3f, _area.flyingSLA);
        StartCoroutine(MineLevel11(32, 8f, 8f, 1f, Color.cyan, mine, mine2, mine3));

    }

    IEnumerator MineLevel11(int droneCount, float delay, float speed, float size, Color color, GameObject mine, GameObject mine2, GameObject mine3)
    {
        while (true)
        {
            _spawnDrone.StraightFlying360Drones(droneCount, speed, size, color, mine.transform.position);
            yield return new WaitForSeconds(delay / 3);
            _spawnDrone.StraightFlying360Drones(droneCount, speed, size, color, mine2.transform.position);
            yield return new WaitForSeconds(delay / 3);
            _spawnDrone.StraightFlying360Drones(droneCount, speed, size, color, mine3.transform.position);
            yield return new WaitForSeconds(delay / 3);
            if (delay > 3f) { delay -= delay * 0.1f; }
        }
    }
}