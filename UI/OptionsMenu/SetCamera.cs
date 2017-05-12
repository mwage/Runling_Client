using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Launcher;
using Assets.Scripts.Players;

namespace Assets.Scripts.UI.OptionsMenu
{
    public class SetCamera : MonoBehaviour
    {
        public GameObject SetHotkeyPrefab;
        public GameObject HotkeyList;
        public GameObject SliderPrefab;
        public Camera Camera;

        private Vector3 _currentPosition;
        private Slider _cameraZoomSlider;
        private Slider _cameraAngleSlider;

        private void OnEnable()
        {
            if (Camera != null)
            {
                _currentPosition = Camera.GetComponent<CameraMovement>().GetLookAtPosition();
            }
            if (HotkeyList.transform.childCount == 0) // dont add the same buttons when switching between submenus
            {
                SubmenuBuilder.AddButton(HotkeyAction.CameraUp, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.CameraDown, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.CameraLeft, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.CameraRight, SetHotkeyPrefab, HotkeyList);
                _cameraZoomSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Zoom");
                _cameraAngleSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Angle");
                LoadCameraOptions();
                _cameraZoomSlider.onValueChanged.AddListener(SetCameraZoom);
                _cameraAngleSlider.onValueChanged.AddListener(SetCameraAngle);
            }
        }

        // Update is called once per frame
        void Update()
        {
            InputManager.RebindHotkeyIfNeed();
        }

        public void LoadCameraOptions()
        {
            if (_cameraZoomSlider != null && _cameraAngleSlider)
            {
                _cameraZoomSlider.value = GameControl.CameraZoom;
                _cameraAngleSlider.value = GameControl.CameraAngle;
                _cameraAngleSlider.maxValue = 90;
            }
        }

        public void SaveCameraOptions()
        {
            GameControl.CameraZoom = _cameraZoomSlider.value;
            GameControl.CameraAngle = _cameraAngleSlider.value;
            PlayerPrefs.SetFloat("CameraZoom", _cameraZoomSlider.value);
            PlayerPrefs.SetFloat("CameraAngle", _cameraAngleSlider.value);
            PlayerPrefs.Save();
        }

        public void SetCameraZoom(float zoom)
        {
            if (Camera != null)
            {
                Camera.transform.position = new Vector3(_currentPosition.x, zoom * Mathf.Sin(_cameraAngleSlider.value * Mathf.PI / 180), _currentPosition.z - zoom * Mathf.Cos(_cameraAngleSlider.value * Mathf.PI / 180));
                Camera.transform.rotation = Quaternion.Euler(_cameraAngleSlider.value, 0, 0);
            }
        }

        public void SetCameraAngle(float angle)
        {
            if (Camera != null)
            {
                Camera.transform.position = new Vector3(_currentPosition.x, _cameraZoomSlider.value * Mathf.Sin(angle * Mathf.PI / 180), 
                    _currentPosition.z - _cameraZoomSlider.value * Mathf.Cos(angle * Mathf.PI / 180));
                Camera.transform.rotation = Quaternion.Euler(angle, 0, 0);
            }
        }
    }
}
