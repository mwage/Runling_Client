using System.Collections;
using Assets.Scripts.Drones;
using Assets.Scripts.SLA;
using Assets.Scripts.SLA.Levels;
using UnityEngine;

public class Level9SLA : ALevel
{
    public Level9SLA(LevelManagerSLA manager) : base(manager)
    {
    }

    public override float GetMovementSpeed()
    {
        return 11;
    }

    public override void CreateDrones()
    {
        // Spawn drones (dronecount/delay, speed, size, color)
        DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1.5f, Color.red), 15, 9f);

        // Spawn chaser
        var player = Manager._initializeGameSLA.NewPlayer;
        DroneFactory.SpawnDrones(new ChaserDrone(9f, 1.2f, Color.yellow, player));
        
        // Spawn green drones (initial delay, size)
        DroneFactory.StartCoroutine(GreenDronesLevel9(4f, 1.5f));
    }
    
    IEnumerator GreenDronesLevel9(float delay, float size)
    {
        var droneCount = 0;
        while (true)
        {
            DroneFactory.SpawnDrones(new StraightFlying360Drone(8f, size, Color.green, 16 + 2 * droneCount, true));
            yield return new WaitForSeconds(delay);
            DroneFactory.SpawnDrones(new StraightFlying360Drone(8f, size, Color.green, 16 + 2 * droneCount, false));
            yield return new WaitForSeconds(delay);
            if (delay > 1f) { delay -= delay * 0.1f; }
            if (droneCount < 15) { droneCount++; }
        }
    }
}
