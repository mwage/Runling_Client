using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTestSLA : MonoBehaviour
{

    // attach scripts
    public SpawnDrone _spawnDrone;
    public AddDrone _addDrone;
    public BoundariesSLA _area;
    public GameObject flyingOnewayDrone;
    public Chaser _chaser;
    public InitializeGameSLA _initializeGameSLA;


    public void LoadDrones()
    {
        // Spawn drones (dronecount/delay, speed, size, color)
        _spawnDrone.RandomBouncingDrone(10, 5f, 1f, Color.blue, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(4f, 5f, 1f, Color.blue, _area.bouncingSLA);
        _spawnDrone.RandomBouncingDrone(8, 5f, 1.5f, Color.red, _area.bouncingSLA);
        _addDrone.RandomBouncingDrone(8f, 5f, 1.5f, Color.red, _area.bouncingSLA);
        GameObject chaser = Instantiate(flyingOnewayDrone, new Vector3(0, 0.6f, 0), Quaternion.identity);
        GameObject player = _initializeGameSLA.newPlayer;
        _chaser.ChaserDrone(chaser, player, 4f, 1.5f, Color.yellow);

        // Spawn green drones (initial delay, inbetween delay, size)
        StartCoroutine(GreenDronesLevel8(5f, 1.5f, Color.green));
    }

    IEnumerator GreenDronesLevel8(float delay, float size, Color color)
    {
        Vector3 startPos = new Vector3(0, 0.6f, _area.flyingSLA.bottomBoundary + (0.5f + size / 2));
        Vector3 startPos2 = new Vector3(0, 0.6f, _area.flyingSLA.topBoundary - (0.5f + size / 2));
        int droneCount = 0;

        while (true)
        {
            _spawnDrone.DelayedStraightFlying360Drones(16 + 2 * droneCount, 0.2f, 7f, size, color, startPos, 90, false);
            yield return new WaitForSeconds(delay/3);
            _spawnDrone.DelayedStraightFlying360Drones(16 + 2 * droneCount, 0.2f, 7f, size, color, startPos, -90, true);
            yield return new WaitForSeconds(delay);
            _spawnDrone.DelayedStraightFlying360Drones(16 + 2 * droneCount, 0.2f, 7f, size, color, startPos2, 90, true);
            yield return new WaitForSeconds(delay /3);
            _spawnDrone.DelayedStraightFlying360Drones(16 + 2 * droneCount, 0.2f, 7f, size, color, startPos2, -90, false);
            yield return new WaitForSeconds(delay);
            if (delay > 1f) { delay -= delay * 0.1f; }
            if (droneCount < 15) { droneCount++; }
        }
    }
}