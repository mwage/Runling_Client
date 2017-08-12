using Launcher;
using TrueSync;

namespace MP.TSGame.Drones
{
    public class DroneDirection {

        // Generates a random direction
        public static FP RandomDirection(FP restrictedZone, float? coneRange = null)
        {
            bool droneAngle;
            FP randomDir;
            var range = coneRange ?? 360;

            do
            {
                randomDir = GameControl.GameState.Random.Next(0f, range);

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
