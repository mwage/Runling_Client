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
        // Spawn Bouncing Drones
        DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1.5f, Color.red), 15, 9f);

        // Spawn Chaser Drone
        var player = Manager.InitializeGameSLA.NewPlayer;
        DroneFactory.SpawnDrones(new ChaserDrone(7f, 1.1f, Color.yellow, player));
        
        // Spawn Green Drones
        DroneFactory.StartCoroutine(GreenDronesLevel9(4f, 16, 8f, 1.5f, Color.green, 0.1f, 1f, 1, 16));
    }

    IEnumerator GreenDronesLevel9(float delay, int initialDroneCount, float speed, float size, Color color, float reduceDelay, float minDelay, int droneIncrease, int maxDrones)
    {
        var droneCount = 0;
        while (true)
        {
            DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, color, initialDroneCount + 2 * droneCount, true));
            yield return new WaitForSeconds(delay);
            DroneFactory.SpawnDrones(new StraightFlying360Drone(speed, size, color, initialDroneCount + 2 * droneCount, false));
            yield return new WaitForSeconds(delay);
            if (delay > minDelay) { delay -= delay * reduceDelay; }
            if (droneCount < maxDrones) { droneCount += droneIncrease; }
        }
    }
}
