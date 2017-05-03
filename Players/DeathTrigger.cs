using System.Runtime.Remoting.Services;
using Assets.Scripts.Launcher;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private bool _finishedLevel;


    // Ttrigger
    void OnTriggerStay(Collider other)
    {
        // Enter Finishzone
        if (other.tag == "Finish" && !_finishedLevel)
        {
            GameControl.FinishedLevel = true;
            _finishedLevel = true;
        }

        // Enter Safezone
        if (other.tag == "SafeZone" && !GameControl.IsInvulnerable)
        {
            GameControl.IsInvulnerable = true;
        }

        // Death Trigger
        if (other.tag == "Enemy" && !GameControl.IsInvulnerable && !GameControl.GodModeActive)
        {
            GameControl.Dead = true;
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
