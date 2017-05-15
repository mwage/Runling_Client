using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Launcher;
using Assets.Scripts.Players.Camera;

namespace Assets.Scripts.UI.OptionsMenu
{
    public class SetCamera : MonoBehaviour
    {
        public GameObject SetHotkeyPrefab;
        public GameObject HotkeyList;
        public GameObject SliderPrefab;
        public GameObject SelectionPrefab;

        public GameObject CameraHandle;
        private Camera _camera;
        
        private CameraMovement _cameraMovement;
        private CameraHandleMovement _cameraHandleMovement;
        private Vector3 _currentPosition;
        private Slider _cameraZoomSlider;
        private Slider _cameraAngleSlider;
        private Slider _cameraSpeedSlider;
        private Toggle _cameraFollowSelection;

        private void OnEnable()
        {
            if (CameraHandle != null)
            {
                _camera = CameraHandle.GetComponentInChildren<Camera>();
                _cameraMovement = _camera.GetComponent<CameraMovement>();
                _cameraHandleMovement = CameraHandle.GetComponent<CameraHandleMovement>();
                _currentPosition = CameraHandle.GetComponent<CameraHandleMovement>().GetWatchedPoint();
            }
            if (HotkeyList.transform.childCount == 0) // dont add the same buttons when switching between submenus
            {
                SubmenuBuilder.AddButton(HotkeyAction.ZoomMore, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.ZoomLess, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.RotateLeft, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.RotateRight, SetHotkeyPrefab, HotkeyList);

                _cameraZoomSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Zoom", Settings.CameraZoom.Val, Settings.CameraZoom.Min, Settings.CameraZoom.Max, SetCameraZoom);
                _cameraAngleSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Angle", Settings.CameraAngle.Val, Settings.CameraAngle.Min, Settings.CameraAngle.Max, SetCameraAngle);
                _cameraSpeedSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Speed", Settings.CameraSpeed.Val, Settings.CameraSpeed.Min, Settings.CameraSpeed.Max, SetCameraSpeed);
                _cameraFollowSelection = SubmenuBuilder.AddSelection(SelectionPrefab, HotkeyList, "Follow", FollowCameraSelection);

                SetSliderValuesFromSettings();
            }
        }

        // Update is called once per frame
        void Update()
        {
            InputManager.RebindHotkeyIfNeed();
        }

        public void FollowCameraSelection(bool on)
        {
            if (_cameraFollowSelection.isOn )
            {
                Settings.CameraFollow = 1; // tru
            }
            else
            {
                Settings.CameraFollow = 0;
            }
        }
        
        public void SetSliderValuesFromSettings()
        {
            if (_cameraZoomSlider != null)
            {
                _cameraZoomSlider.value = Settings.CameraZoom.Val;
                _cameraAngleSlider.value = Settings.CameraAngle.Val;
                _cameraSpeedSlider.value = Settings.CameraSpeed.Val;
                _cameraFollowSelection.isOn = Settings.CameraFollow == 1;
            }
        }
        
        public void SaveCameraOptions()
        {
             /////// change it before
            PlayerPrefs.SetFloat("CameraZoom", Settings.CameraZoom.Val);
            PlayerPrefs.SetFloat("CameraAngle", Settings.CameraAngle.Val);
            PlayerPrefs.SetFloat("CameraSpeed", Settings.CameraSpeed.Val);
            PlayerPrefs.SetInt("CameraFollow", Settings.CameraFollow);
            PlayerPrefs.Save();
        }

        public void SetCameraZoom(float zoom)
        {
            if (_camera != null)
            {
                Settings.CameraZoom.Val = zoom;
                _cameraHandleMovement.SetCameraHandlePosition(_currentPosition);
                _camera.GetComponent<CameraMovement>().SetCameraPitch(Settings.CameraAngle.Val);
            }
        }

        public void SetCameraAngle(float angle)
        {
            if (_camera != null)
            {
                Settings.CameraAngle.Val = angle;
                _cameraHandleMovement.SetCameraHandlePosition(_currentPosition);
                _cameraMovement.SetCameraPitch(Settings.CameraAngle.Val);
            }
        }

        public void SetCameraSpeed(float speed)
        {
            if (_camera != null)
            {
                Settings.CameraSpeed.Val = speed;
            }
        }
    }
}
