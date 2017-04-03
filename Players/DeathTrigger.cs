using Assets.Scripts.Launcher;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    //death trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            GameControl.dead = true;
        }
    }
}
