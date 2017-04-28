using UnityEngine;

namespace Assets.Scripts.UI.OptionsMenu
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

        public void DiscardChanges()
        {
            InputManager.LoadHotkeys();      
            OptionsMenuActive = false;
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }

        public void SaveChanges()
        {
            foreach (string key in InputManager.Hotkeys.Keys)
            {
                int intHotkey = (int)InputManager.Hotkeys[key];

                //Save the keybind
                PlayerPrefs.SetInt(key, intHotkey);
            }
            PlayerPrefs.Save();

            OptionsMenuActive = false;
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }
    }
}
