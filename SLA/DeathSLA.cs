using UnityEngine;

namespace Assets.Scripts.SLA
{
    public class DeathSLA : MonoBehaviour
    {
        public InitializeGameSLA _initializeGameSLA;

        //events following Deathtrigger
        public void Death()
        {
            _initializeGameSLA.Player.SetActive(false);
        }
    }
}
