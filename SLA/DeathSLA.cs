using Launcher;
using UnityEngine;

namespace SLA
{
    public class DeathSLA : MonoBehaviour
    {
        //events following Deathtrigger
        public void Death()
        {
            GameControl.State.Player.SetActive(false);
        }
    }
}
