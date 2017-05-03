using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class DroneDirection {

        // Generates a random direction
        public static float RandomDirection(float restrictedZone)
        {
            bool droneAngle;
            float randomDir;

            do
            {
                randomDir = Random.Range(0f, 360f);

                if (randomDir < restrictedZone | randomDir > 360-restrictedZone) { droneAngle = false; }
                else if (randomDir > 90f-restrictedZone && randomDir < 90f+restrictedZone) { droneAngle = false; }
                else if (randomDir > 180f - restrictedZone && randomDir < 180f + restrictedZone) { droneAngle = false; }
                else if (randomDir > 270f - restrictedZone && randomDir < 270f + restrictedZone) { droneAngle = false; }
                else { droneAngle = true; }

            } while (droneAngle == false);

            return randomDir;
        }
    }
}
