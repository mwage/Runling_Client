using System;
using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.UI.OptionsMenu
{
    public class OptionsMenu : MonoBehaviour
    {

        public GameObject Menu;
        public SetHotkeys SetHotkeys;
        public SetCamera SetCamera;

        public GameObject GeneralHotkeysSubmenu;
        public GameObject CameraSubmenu;
        public GameObject GraphicsSubmenu;

        public bool OptionsMenuActive;

        public void DiscardChanges()
        {
            SubmenuBuilder.DeleteHotkeyPrefabs(SetCamera.HotkeyList); 
            SubmenuBuilder.DeleteHotkeyPrefabs(SetHotkeys.HotkeyList);
            InputManager.LoadHotkeys();
            SetCamera.LoadCameraOptions();
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
                    PlayerPrefs.SetInt(action.ToString(), (int)kc);
            }
            SetCamera.SaveCameraOptions();
            //SetHotkeys.DeleteHotkeyPrefabs(); // noneed i think

            OptionsMenuActive = false;
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }

        public void GeneralHotkeysToggle()
        {
            GeneralHotkeysSubmenu.SetActive(true);
            CameraSubmenu.SetActive(false);
            GraphicsSubmenu.SetActive(false);
        }

        public void CameraToggle()
        {
            GeneralHotkeysSubmenu.SetActive(false);
            CameraSubmenu.SetActive(true);
            GraphicsSubmenu.SetActive(false);
        }

        public void GraphicsToggle()
        {
            GeneralHotkeysSubmenu.SetActive(false);
            CameraSubmenu.SetActive(false);
            GraphicsSubmenu.SetActive(true);
        }

    }
}
