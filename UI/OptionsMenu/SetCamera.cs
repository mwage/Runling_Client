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
                SubmenuBuilder.AddButton(HotkeyAction.ActivateFollow, SetHotkeyPrefab, HotkeyList);


                _cameraZoomSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Zoom", Settings.Instance.CameraZoom.Val, Settings.Instance.CameraZoom.Min, Settings.Instance.CameraZoom.Max, SetCameraZoom);
                _cameraAngleSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Angle", Settings.Instance.CameraAngle.Val, Settings.Instance.CameraAngle.Min, Settings.Instance.CameraAngle.Max, SetCameraAngle);
                _cameraSpeedSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Speed", Settings.Instance.CameraSpeed.Val, Settings.Instance.CameraSpeed.Min, Settings.Instance.CameraSpeed.Max, SetCameraSpeed);
                _cameraFollowSelection = SubmenuBuilder.AddSelection(SelectionPrefab, HotkeyList, "Follow", FollowCameraSelection);


                SetSliderValuesFromSettings();
            }
        }

        // Update is called once per frame
        void Update()
        {
            InputManager.Instance.RebindHotkeyIfNeed();
        }

        public void FollowCameraSelection(bool on)
        {
            if (_cameraFollowSelection.isOn )
            {
                Settings.Instance.FollowEnabled = 1; // tru
            }
            else
            {
                Settings.Instance.FollowEnabled = 0;
            }
        }
        
        public void SetSliderValuesFromSettings()
        {
            if (_cameraZoomSlider != null)
            {

                _cameraZoomSlider.value = Settings.Instance.CameraZoom.Val;
                _cameraAngleSlider.value = Settings.Instance.CameraAngle.Val;
                _cameraSpeedSlider.value = Settings.Instance.CameraSpeed.Val;
                _cameraFollowSelection.isOn = Settings.Instance.FollowEnabled == 1;

            }
        }
        
        public void SaveCameraOptions()
        {
             /////// change it before
            PlayerPrefs.SetFloat("CameraZoom", Settings.Instance.CameraZoom.Val);
            PlayerPrefs.SetFloat("CameraAngle", Settings.Instance.CameraAngle.Val);
            PlayerPrefs.SetFloat("CameraSpeed", Settings.Instance.CameraSpeed.Val);
            PlayerPrefs.SetInt("CameraFollow", Settings.Instance.FollowEnabled);

            PlayerPrefs.Save();
        }

        public void SetCameraZoom(float zoom)
        {
            if (_camera != null)
            {
                Settings.Instance.CameraZoom.Val = zoom;
                _cameraHandleMovement.SetCameraHandlePosition(_currentPosition);
                _camera.GetComponent<CameraMovement>().SetCameraPitch(Settings.Instance.CameraAngle.Val);
            }
        }

        public void SetCameraAngle(float angle)
        {
            if (_camera != null)
            {
                Settings.Instance.CameraAngle.Val = angle;
                _cameraHandleMovement.SetCameraHandlePosition(_currentPosition);
                _cameraMovement.SetCameraPitch(Settings.Instance.CameraAngle.Val);
            }
        }

        public void SetCameraSpeed(float speed)
        {
            if (_camera != null)
            {
                Settings.Instance.CameraSpeed.Val = speed;
            }
        }
    }
}
