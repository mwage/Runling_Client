using System.Collections;
using Assets.Scripts.Drones;
using Assets.Scripts.SLA;
using Assets.Scripts.SLA.Levels;
using UnityEngine;

public class Level9SLA : ALevelSLA
{
    public Level9SLA(LevelManagerSLA manager) : base(manager)
    {
    }

    public override float GetMovementSpeed()
    {
        return 10;
    }

    public override void CreateDrones()
    {
        // Spawn drones (dronecount/delay, speed, size, color)
        DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1.5f, Color.red), 15, 9f);

        // Spawn chaser
        var player = Manager.InitializeGameSLA.NewPlayer;
        DroneFactory.SpawnDrones(new ChaserDrone(9f, 1.2f, Color.yellow, player));
        
        // Spawn green drones (initial delay, size)
        DroneFactory.StartCoroutine(GreenDronesLevel9(4f, 16, 8f, 1.5f, 0.1f, 1f, 1, 16));
    }

    IEnumerator GreenDronesLevel9(float delay, int initialDroneCount, float speed, float size, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
    {
        var droneCount = 0;
        while (true)
        {
            DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.green, initialDroneCount + 2 * droneCount, true));
            yield return new WaitForSeconds(delay);
            DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, Color.green, initialDroneCount + 2 * droneCount, false));
            yield return new WaitForSeconds(delay);
            if (delay > minDelay) { delay -= delay * reduceDelay; }
            if (droneCount < maxDrones) { droneCount += droneIncrease; }
        }
    }
}
