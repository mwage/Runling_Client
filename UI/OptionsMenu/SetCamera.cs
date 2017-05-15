using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Launcher;
using Assets.Scripts.Players.Cameras;

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
            if (_camera != null)
            {
                _camera = CameraHandle.GetComponentInChildren<Camera>();
                _cameraMovement = _camera.GetComponent<CameraMovement>();
                _cameraHandleMovement = CameraHandle.GetComponent<CameraHandleMovement>();
                _currentPosition = CameraHandle.GetComponent<CameraHandleMovement>().GetWatchedPoint();
            }
            if (HotkeyList.transform.childCount == 0) // dont add the same buttons when switching between submenus
            {
                //SubmenuBuilder.AddButton(HotkeyAction.CameraUp, SetHotkeyPrefab, HotkeyList); // need to change "horizontal/vertical" axes to Input.... to use it, but everyone uses wasd...
                //SubmenuBuilder.AddButton(HotkeyAction.CameraDown, SetHotkeyPrefab, HotkeyList);
                //SubmenuBuilder.AddButton(HotkeyAction.CameraLeft, SetHotkeyPrefab, HotkeyList);
                //SubmenuBuilder.AddButton(HotkeyAction.CameraRight, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.ZoomMore, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.ZoomLess, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.RotateLeft, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.RotateRight, SetHotkeyPrefab, HotkeyList);

                _cameraZoomSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Zoom", 50, 10, 100, SetCameraZoom);
                _cameraAngleSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Angle", 90, 20, 90, SetCameraAngle);
                _cameraSpeedSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Speed", 10, 5, 50, SetCameraSpeed);

                _cameraFollowSelection = SubmenuBuilder.AddSelection(SelectionPrefab, HotkeyList, "Follow", FollowCameraSelection);

                SetSliderValuesFromGameControl();
            }
        }

        // Update is called once per frame
        void Update()
        {
            InputManager.RebindHotkeyIfNeed();
        }

        void LateUpdate()
        {

        }

        public void FollowCameraSelection(bool on)
        {
            if (_cameraFollowSelection.isOn )
            {
                GameControl.CameraFollow = 1; // tru
            }
            else
            {
                GameControl.CameraFollow = 0;
            }
        }

        public void SetSliderValuesFromGameControl()
        {
            if (_cameraZoomSlider != null)
            {
                _cameraZoomSlider.value = GameControl.CameraZoom.Val;
                _cameraAngleSlider.value = GameControl.CameraAngle.Val;
                _cameraSpeedSlider.value = GameControl.CameraSpeed.Val;
            }
        }

        public void SaveCameraOptions()
        {
             /////// change it before
            PlayerPrefs.SetFloat("CameraZoom", GameControl.CameraZoom.Val);
            PlayerPrefs.SetFloat("CameraAngle", GameControl.CameraAngle.Val);
            PlayerPrefs.SetFloat("CameraSpeed", GameControl.CameraSpeed.Val);
            PlayerPrefs.SetInt("CameraFollow", GameControl.CameraFollow);

            PlayerPrefs.Save();
        }

        public void SetCameraZoom(float zoom)
        {
            if (_camera != null)
            {
                GameControl.CameraZoom.Val = zoom;
                _cameraHandleMovement.SetCameraHandlePosition(_currentPosition);
                _camera.GetComponent<CameraMovement>().SetCameraPitch(GameControl.CameraAngle.Val);
            }
        }

        public void SetCameraAngle(float angle)
        {
            if (_camera != null)
            {
                GameControl.CameraAngle.Val = angle;
                _cameraHandleMovement.SetCameraHandlePosition(_currentPosition);
                _cameraMovement.SetCameraPitch(GameControl.CameraAngle.Val);
            }
        }

        public void SetCameraSpeed(float speed)
        {
            if (_camera != null)
            {
                GameControl.CameraSpeed.Val = speed;
            }
        }
        
    }
}
