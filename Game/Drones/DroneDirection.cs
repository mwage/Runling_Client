using UnityEngine;

namespace Game.Scripts.Drones
{
    public class DroneDirection {

        // Generates a random direction
        public static float RandomDirection(float restrictedZone, float coneRange)
        {
            bool droneAngle;
            float randomDir;
            var range = coneRange;

            do
            {
                randomDir = Random.Range(0f, range);

                if (randomDir < restrictedZone | randomDir > range-restrictedZone) { droneAngle = false; }
                else if (randomDir > range/4-restrictedZone && randomDir < range/4+restrictedZone) { droneAngle = false; }
                else if (randomDir > range/2 - restrictedZone && randomDir < range/2 + restrictedZone) { droneAngle = false; }
                else if (randomDir > range*3/4 - restrictedZone && randomDir < range*3/4 + restrictedZone) { droneAngle = false; }
                else { droneAngle = true; }

            } while (droneAngle == false);

            return randomDir;
        }
    }
}
