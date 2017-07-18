
using Characters;
using Launcher;
using RLR.Levels;
using UnityEngine;

namespace Players
{
    public class PlayerTriggerManager
    {
        public bool IsSafeZoneVisitedEarlier(GameObject currentSafeZone)
        {
            if (GameControl.MapState.SafeZones.Contains(currentSafeZone)) // always should contain
            {
                var currentSafeZoneIdx = GameControl.MapState.SafeZones.IndexOf(currentSafeZone);
                if (GameControl.MapState.VisitedSafeZones[currentSafeZoneIdx])
                    return true; // you have been here, no exp for you

                return false;
            }
            else
            {
                Debug.Log("you gave not a safe zone to this function");
                return false;
            }
        }
    }
}