using UnityEngine;

namespace Assets.Scripts.UI
{
    public class OptionsMenu : MonoBehaviour {

        public GameObject Menu;

        public bool OptionsMenuActive;

        public void BackToMenu()
        {
            OptionsMenuActive = false;
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }
    }
}
