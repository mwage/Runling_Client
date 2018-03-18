using Client.Scripts.IngameCamera;
using Client.Scripts.Launcher;
using System;
using UnityEngine;

namespace Client.Scripts.UI.Menus.OptionsMenu
{
    public class OptionsMenu : AMenu
    {
        public SetHotkeys SetHotkeys;
        public SetCamera SetCamera;
        public CameraMovement CameraMovement;

        public GameObject GeneralHotkeysSubmenu;
        public GameObject CameraSubmenu;
        public GameObject GraphicsSubmenu;

        private MenuManager _menuManager;

        private void Awake()
        {
            _menuManager = transform.parent.GetComponent<MenuManager>();
        }

        private void OnEnable()
        {
            _menuManager.ActiveMenu?.gameObject.SetActive(false);
            _menuManager.ActiveMenu = this;
        }

        #region Buttons
        
        public void DiscardChanges()
        {
            SubmenuBuilder.DeleteHotkeyPrefabs(SetCamera.HotkeyList); 
            SubmenuBuilder.DeleteHotkeyPrefabs(SetHotkeys.HotkeyList);
            GameControl.HotkeyMapping.LoadHotkeys();

            if (CameraMovement != null && CameraMovement != null)
            {
                var watchedPoint = CameraMovement.GetWatchedPoint();
                CameraMovement.SetCameraPosition(watchedPoint);
                CameraMovement.SetCameraPitch(GameControl.Settings.CameraAngle.Val);
            }

            GameControl.Settings.LoadSettings();
            _menuManager.Menu.SetActive(true);
        }

        public void SaveChanges()
        {
            foreach (HotkeyAction action in Enum.GetValues(typeof(HotkeyAction)))
            {
                var kc = GameControl.HotkeyMapping.GetHotkey(action);
                if (kc != null)
                    PlayerPrefs.SetInt(action.ToString(), (int)kc);
            }
            SetCamera.SaveCameraOptions();

            _menuManager.Menu.SetActive(true);
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
        #endregion

        public override void Back()
        {
            DiscardChanges();
        }
    }
}
