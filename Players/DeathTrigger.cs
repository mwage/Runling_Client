using System.Collections;
using System.Collections.Generic;
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
