using UnityEngine;

namespace Assets.Scripts.UI
{
    public class OptionsMenu : MonoBehaviour {

        public GameObject menu;

        public bool optionsMenuActive;

        public void BackToMenu()
        {
            optionsMenuActive = false;
            gameObject.SetActive(false);
            menu.gameObject.SetActive(true);
        }
    }
}
