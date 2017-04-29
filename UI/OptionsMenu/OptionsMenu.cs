using System;
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

            OptionsMenuActive = false;
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }

        public void SaveChanges()
        {
            foreach (HotkeyAction action in Enum.GetValues(typeof(HotkeyAction)))
            {
                var kc = InputManager.Instance.GetHotkey(action);
                if (kc != null)
                    PlayerPrefs.SetInt(action.ToString(), (int) kc);
            }
            PlayerPrefs.Save();
            SetHotkeys.DeleteHotkeyPrefabs();

            OptionsMenuActive = false;
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }

    }
}
