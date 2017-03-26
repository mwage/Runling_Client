using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneDirection : MonoBehaviour {

    // Generates a random direction
    public float RandomDirection(float restrictedZone)
    {
        bool droneAngle = false;
        float randomDir;

        do
        {
            randomDir = Random.Range(0f, 360f);
            
            if (randomDir < restrictedZone | randomDir > 360-restrictedZone) { droneAngle = false; }
            if (randomDir > 90f-restrictedZone && randomDir < 90f+restrictedZone) { droneAngle = false; }
            if (randomDir > 180f - restrictedZone && randomDir < 180f + restrictedZone) { droneAngle = false; }
            if (randomDir > 270f - restrictedZone && randomDir < 270f + restrictedZone) { droneAngle = false; }
            else { droneAngle = true; }

        } while (droneAngle == false);

        return randomDir;
    }
}
