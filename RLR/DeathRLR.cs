using UnityEngine;

namespace Assets.Scripts.RLR
{
    public class DeathRLR : MonoBehaviour
    {
        public InitializeGameRLR InitializeGameRLR;

        //events following Deathtrigger
        public void Death()
        {
            InitializeGameRLR.Player.SetActive(false);
        }
    }
}
