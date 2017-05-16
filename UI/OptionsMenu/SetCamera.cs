using UnityEngine;
using UnityEngine.UI;
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

                _cameraZoomSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Zoom", GameControl.Instance.Settings.CameraZoom.Val, GameControl.Instance.Settings.CameraZoom.Min, GameControl.Instance.Settings.CameraZoom.Max, SetCameraZoom);
                _cameraAngleSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Angle", GameControl.Instance.Settings.CameraAngle.Val, GameControl.Instance.Settings.CameraAngle.Min, GameControl.Instance.Settings.CameraAngle.Max, SetCameraAngle);
                _cameraSpeedSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Speed", GameControl.Instance.Settings.CameraSpeed.Val, GameControl.Instance.Settings.CameraSpeed.Min, GameControl.Instance.Settings.CameraSpeed.Max, SetCameraSpeed);
                _cameraFollowSelection = SubmenuBuilder.AddSelection(SelectionPrefab, HotkeyList, "Follow", FollowCameraSelection);

                SetSliderValuesFromSettings();
            }
        }

        // Update is called once per frame
        void Update()
        {
            GameControl.Instance.InputManager.RebindHotkeyIfNeed();
        }

        public void FollowCameraSelection(bool on)
        {
            if (_cameraFollowSelection.isOn )
            {
                GameControl.Instance.Settings.FollowEnabled = 1; // tru
            }
            else
            {
                GameControl.Instance.Settings.FollowEnabled = 0;
            }
        }
        
        public void SetSliderValuesFromSettings()
        {
            if (_cameraZoomSlider != null)
            {

                _cameraZoomSlider.value = GameControl.Instance.Settings.CameraZoom.Val;
                _cameraAngleSlider.value = GameControl.Instance.Settings.CameraAngle.Val;
                _cameraSpeedSlider.value = GameControl.Instance.Settings.CameraSpeed.Val;
                _cameraFollowSelection.isOn = GameControl.Instance.Settings.FollowEnabled == 1;

            }
        }
        
        public void SaveCameraOptions()
        {
             /////// change it before
            PlayerPrefs.SetFloat("CameraZoom", GameControl.Instance.Settings.CameraZoom.Val);
            PlayerPrefs.SetFloat("CameraAngle", GameControl.Instance.Settings.CameraAngle.Val);
            PlayerPrefs.SetFloat("CameraSpeed", GameControl.Instance.Settings.CameraSpeed.Val);
            PlayerPrefs.SetInt("CameraFollow", GameControl.Instance.Settings.FollowEnabled);

            PlayerPrefs.Save();
        }

        public void SetCameraZoom(float zoom)
        {
            if (_camera != null)
            {
                GameControl.Instance.Settings.CameraZoom.Val = zoom;
                _cameraHandleMovement.SetCameraHandlePosition(_currentPosition);
                _camera.GetComponent<CameraMovement>().SetCameraPitch(GameControl.Instance.Settings.CameraAngle.Val);
            }
        }

        public void SetCameraAngle(float angle)
        {
            if (_camera != null)
            {
                GameControl.Instance.Settings.CameraAngle.Val = angle;
                _cameraHandleMovement.SetCameraHandlePosition(_currentPosition);
                _cameraMovement.SetCameraPitch(GameControl.Instance.Settings.CameraAngle.Val);
            }
        }

        public void SetCameraSpeed(float speed)
        {
            if (_camera != null)
            {
                GameControl.Instance.Settings.CameraSpeed.Val = speed;
            }
        }
    }
}
