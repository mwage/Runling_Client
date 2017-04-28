using UnityEngine;

namespace Assets.Scripts.RLR
{
    public class DeathRLR : MonoBehaviour
    {
        public InitializeGameRLR _initializeGameRLR;

        //events following Deathtrigger
        public void Death()
        {
            _initializeGameRLR.Player.SetActive(false);
        }
    }
}
