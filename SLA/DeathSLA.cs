using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.SLA
{
    public class DeathSLA : MonoBehaviour
    {
        //events following Deathtrigger
        public void Death()
        {
            GameControl.Instance.State.Player.SetActive(false);
        }
    }
}
