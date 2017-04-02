using UnityEngine;

namespace Assets.Scripts.UI.Main_Menu
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject optionsMenu;
        public GameObject SLAMenu;
        public OptionsMenu _optionsMenu;
        public SLAMenu _SLAMenu;

        public void SLA()
        {
            gameObject.SetActive(false);
            SLAMenu.gameObject.SetActive(true);
            _SLAMenu.SLAMenuActive = true;
        }

        public void Options()
        {
            gameObject.SetActive(false);
            optionsMenu.gameObject.SetActive(true);
            _optionsMenu.optionsMenuActive = true;
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
