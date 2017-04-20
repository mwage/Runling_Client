using Assets.Scripts.Launcher;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    // Ttrigger
    void OnTriggerStay(Collider other)
    {
        // Death Trigger
        if (other.tag == "Enemy" && !GameControl.IsInvulnerable && !GameControl.GodModeActive)
        {
            GameControl.Dead = true;
        }

        // Enter Safezone
        if (other.tag == "SafeZone"&& !GameControl.IsInvulnerable)
        {
            GameControl.IsInvulnerable = true;
        }
    }

    // Leave Safezone
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "SafeZone")
        {
            GameControl.IsInvulnerable = false;
        }
    }
}
