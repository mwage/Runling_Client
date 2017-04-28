using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.UI.OptionsMenu
{
    public class OptionsMenu : MonoBehaviour {

        public GameObject Menu;
        public SetHotkeys SetHotkeys;
        

        public bool OptionsMenuActive;

        public void DiscardChanges()
        {
            SetHotkeys.DeleteHotkeyPrefabs();
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
            SetHotkeys.DeleteHotkeyPrefabs();

            OptionsMenuActive = false;
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }

    }
}
