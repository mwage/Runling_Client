using UnityEngine;

namespace Assets.Scripts.SLA
{
    public class DeathSLA : MonoBehaviour
    {
        public InitializeGameSLA InitializeGameSLA;

        //events following Deathtrigger
        public void Death()
        {
            InitializeGameSLA.Player.SetActive(false);
        }
    }
}
