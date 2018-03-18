using Client.Scripts.IngameCamera;
using Client.Scripts.Launcher;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.UI.Menus.OptionsMenu
{
    public class SetCamera : MonoBehaviour
    {
        public GameObject SetHotkeyPrefab;
        public GameObject HotkeyList;
        public GameObject SliderPrefab;
        public GameObject SelectionPrefab;

        public GameObject Camera;

        private Camera _camera;
        private CameraMovement _CameraMovement;
        private Vector3 _currentPosition;
        private Slider _cameraZoomSlider;
        private Slider _cameraAngleSlider;
        private Slider _cameraSpeedSlider;
        private Toggle _cameraFollowSelection;
        private Toggle _hideMiniMapSelection;

        private void OnEnable()
        {
            if (Camera != null)
            {
                _camera = Camera.GetComponentInChildren<Camera>();
                _CameraMovement = Camera.GetComponent<CameraMovement>();
                _currentPosition = Camera.GetComponent<CameraMovement>().GetWatchedPoint();
            }
            if (HotkeyList.transform.childCount == 0) // dont add the same buttons when switching between submenus
            {
                SubmenuBuilder.AddButton(HotkeyAction.ZoomMore, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.ZoomLess, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.RotateLeft, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.RotateRight, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.ActivateFollow, SetHotkeyPrefab, HotkeyList);

                _cameraZoomSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Zoom",  GameControl.Settings.CameraZoom.Min, GameControl.Settings.CameraZoom.Max, SetCameraZoom);
                _cameraAngleSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Angle",  GameControl.Settings.CameraAngle.Min, GameControl.Settings.CameraAngle.Max, SetCameraAngle);
                _cameraSpeedSlider = SubmenuBuilder.AddSlider(SliderPrefab, HotkeyList, "Camera Speed",  GameControl.Settings.CameraSpeed.Min, GameControl.Settings.CameraSpeed.Max, SetCameraSpeed);
                _cameraFollowSelection = SubmenuBuilder.AddSelection(SelectionPrefab, HotkeyList, "Follow", FollowCameraSelection);
                _hideMiniMapSelection = SubmenuBuilder.AddSelection(SelectionPrefab, HotkeyList, "Hide Minimap",HideMiniMapSelection);

                SetSliderValuesFromSettings();
            }
        }

        // Update is called once per frame
        private void Update()
        {
            GameControl.HotkeyMapping.RebindHotkeyIfNeed();
        }

        public void FollowCameraSelection(bool on)
        {
            GameControl.Settings.FollowEnabled = on;
        }

        public void HideMiniMapSelection(bool on)
        {
            GameControl.Settings.HideMiniMap = on;
        }

        public void SetSliderValuesFromSettings()
        {
            if (_cameraZoomSlider != null)
            {
                _cameraZoomSlider.value = GameControl.Settings.CameraZoom.Val;
                _cameraAngleSlider.value = GameControl.Settings.CameraAngle.Val;
                _cameraSpeedSlider.value = GameControl.Settings.CameraSpeed.Val;
                _cameraFollowSelection.isOn = GameControl.Settings.FollowEnabled;
                _hideMiniMapSelection.isOn = GameControl.Settings.HideMiniMap;
            }
        }
        
        public void SaveCameraOptions()
        {
             /////// change it before
            PlayerPrefs.SetFloat("CameraZoom", GameControl.Settings.CameraZoom.Val);
            PlayerPrefs.SetFloat("CameraAngle", GameControl.Settings.CameraAngle.Val);
            PlayerPrefs.SetFloat("CameraSpeed", GameControl.Settings.CameraSpeed.Val);
            PlayerPrefs.SetInt("FollowEnabled", GameControl.Settings.FollowEnabled ? 1: 0);
            PlayerPrefs.SetInt("HideMiniMap", GameControl.Settings.HideMiniMap ? 1 : 0);

            PlayerPrefs.Save();
        }

        public void SetCameraZoom(float zoom)
        {
            if (_camera != null)
            {
                GameControl.Settings.CameraZoom.Val = zoom;
                _CameraMovement.SetCameraPosition(_currentPosition);
                _CameraMovement.SetCameraPitch(GameControl.Settings.CameraAngle.Val);
            }
        }

        public void SetCameraAngle(float angle)
        {
            if (_camera != null)
            {
                GameControl.Settings.CameraAngle.Val = angle;
                _CameraMovement.SetCameraPosition(_currentPosition);
                _CameraMovement.SetCameraPitch(GameControl.Settings.CameraAngle.Val);
            }
        }

        public void SetCameraSpeed(float speed)
        {
            if (_camera != null)
            {
                GameControl.Settings.CameraSpeed.Val = speed;
            }
        }
    }
}
