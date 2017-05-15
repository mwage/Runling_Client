using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Launcher;
using Assets.Scripts.UI.OptionsMenu;


namespace Assets.Scripts.Players.Cameras
{
    public class CameraHandleMovement : MonoBehaviour 
        // the scipt respons on camera position. It ensure rotation around Y axis (yaw), but doenst make around X (pitch) direcly [thats makes CameraMovement]
        // CameraHandle Object allows to use arrows to move properly with Camera with any Y-axis rotation.
    {
        public SetCamera SetCamera;
        private GameObject _player;

        void Awake()
        {
            PlayerPrefs.SetFloat("CameraZoom", PlayerPrefs.GetFloat("CameraZoom") > 0.01 ? PlayerPrefs.GetFloat("CameraZoom") : GameControl.CameraZoom.Def);
            PlayerPrefs.SetFloat("CameraAngle", PlayerPrefs.GetFloat("CameraAngle") > 0.01 ? PlayerPrefs.GetFloat("CameraAngle") : GameControl.CameraAngle.Def);
            PlayerPrefs.SetFloat("CameraSpeed", PlayerPrefs.GetFloat("CameraSpeed") > 0.01 ? PlayerPrefs.GetFloat("CameraSpeed") : GameControl.CameraSpeed.Def);

            GameControl.CameraZoom.Val = PlayerPrefs.GetFloat("CameraZoom");
            GameControl.CameraAngle.Val = PlayerPrefs.GetFloat("CameraAngle");
            GameControl.CameraSpeed.Val = PlayerPrefs.GetFloat("CameraSpeed");
            GameControl.CameraFollow = PlayerPrefs.GetInt("CameraFollow");
        }

        void Start()
        {
            SetCameraHandlePosition(GetWatchedPoint());  
        }

        // Update is called once per frame
        void Update()
        {
        }

        void LateUpdate()
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            float moveX = inputX * GameControl.CameraSpeed.Val * Time.deltaTime;
            float moveY = inputY * GameControl.CameraSpeed.Val * Time.deltaTime;
            transform.position += (transform.forward * moveY + transform.right * moveX);

            if (InputManager.Instance.GetButtonDown(HotkeyAction.RotateLeft))
            {
                RotateCameraYAxis(-90F);
            }
            if (InputManager.Instance.GetButtonDown(HotkeyAction.RotateRight))
            {
                RotateCameraYAxis(90F);
            }
            if (InputManager.Instance.GetButtonDown(HotkeyAction.ZoomMore))
            {
                ZoomMore();
                PlayerPrefs.SetFloat("CameraZoom", GameControl.CameraZoom.Val);
            }
            if (InputManager.Instance.GetButtonDown(HotkeyAction.ZoomLess))
            {
                ZoomLess();
                PlayerPrefs.SetFloat("CameraZoom", GameControl.CameraZoom.Val);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F))        /////// INPUT NOT WORKING PROPERLY
            {
                GameControl.CameraFollow = (GameControl.CameraFollow + 1) % 2;
            }
            if (GameControl.CameraFollow == 1)
            {
                _player = GameObject.Find("Manticore(Clone)");
                if (_player != null) // maybe be bad for multiplayer)     need refactor of player
                {
                    SetCameraHandlePosition(_player.transform.position);
                }
            }
        }

        private void ZoomMore()
        {
            Vector3 watchedPoint = GetWatchedPoint();
            GameControl.CameraZoom.Decrease(10);
            SetCameraHandlePosition(watchedPoint);
        }

        private void ZoomLess()
        {
            Vector3 watchedPoint = GetWatchedPoint();
            GameControl.CameraZoom.Increase(10);
            SetCameraHandlePosition(watchedPoint);
        }

        public void SetCameraHandlePosition(Vector3 watchedPoint)
        {
            if (transform.GetComponentInChildren<Camera>() != null)
            {
                transform.position = new Vector3(
                    watchedPoint.x - transform.forward.x * GameControl.CameraZoom.Val * Mathf.Cos(GameControl.CameraAngle.Val * Mathf.PI / 180),
                    GameControl.CameraZoom.Val * Mathf.Sin(GameControl.CameraAngle.Val * Mathf.PI / 180),
                    watchedPoint.z - transform.forward.z * GameControl.CameraZoom.Val * Mathf.Cos(GameControl.CameraAngle.Val * Mathf.PI / 180));
            }
        }

        public Vector3 GetWatchedPoint()
        {
            return new Vector3(
                transform.localPosition.x + transform.forward.x * GameControl.CameraZoom.Val * Mathf.Cos(GameControl.CameraAngle.Val * Mathf.PI / 180),
                0F,
                transform.localPosition.z + transform.forward.z * GameControl.CameraZoom.Val * Mathf.Cos(GameControl.CameraAngle.Val * Mathf.PI / 180));
        }

        private void RotateCameraYAxis(float degrees)
        {
            Vector3 watchedPoint = GetWatchedPoint();
            transform.Rotate(0F, degrees, 0F);
            SetCameraHandlePosition(watchedPoint);
        }
    }
}





