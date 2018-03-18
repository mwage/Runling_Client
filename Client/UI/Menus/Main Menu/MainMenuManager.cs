using Client.Scripts.Launcher;
using Client.Scripts.Network;
using Client.Scripts.Network.Chat;
using Client.Scripts.Network.Rooms;
using Client.Scripts.UI.Menus.RLR_Menus;
using Client.Scripts.UI.Menus.SLA_Menus;
using DarkRift.Client;
using Game.Scripts.GameSettings;
using Game.Scripts.Network.DarkRiftTags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.UI.Menus.Main_Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        public MainMenu MainMenu;
        public SLAMenu SLAMenu;
        public HighScoreMenuSLA HighScoreMenuSLA;
        public RLRMenu RLRMenu;
        public HighScoreMenuRLR HighScoreMenuRLR;
        public SoloMenu SoloMenu;
        public MultiplayerMenu MultiplayerMenu;
        public SceneLoader SceneLoader;
        public ChatWindowManager ChatWindowManager;

        #region CameraVariables
        public Camera Camera;
        public Vector3 CameraPosMainMenu;
        public Vector3 CameraPosRLR;
        public Vector3 CameraPosSLA;
        private Vector3 _targetPos;
        public Quaternion CameraRotMainMenu;
        public Quaternion CameraRotSLA;
        public Quaternion CameraRotRLR;
        private Quaternion _targetRot;
        private float _distance;
        private float _angle;
        private float _initializationTime;
        private float _currentPos;
        private float _currentRot;
        private float _cameraSpeed;
        private Vector3 _oldPos;
        private Quaternion _oldRot;
        #endregion

        private void Awake()
        {
            #region CameraVariables
            CameraPosMainMenu = Camera.transform.position;
            CameraPosRLR = new Vector3(0, 35, 70);
            CameraPosSLA = new Vector3(0, 35, 60);
            CameraRotMainMenu = Camera.transform.rotation;
            CameraRotSLA = Quaternion.Euler(40, 0, 0);
            CameraRotRLR = Quaternion.Euler(40, 180, 0);
            _targetPos = Camera.transform.position;
            _targetRot = Camera.transform.rotation;
            _cameraSpeed = 100;
            #endregion

            RoomManager.Instance.CurrentRoom = null;
            if (MainClient.Instance.Connected)
            {
                MainClient.Instance.MessageReceived += OnDataHandler;
                RoomManager.onStartGame += StartGame;
            }
        }

        private void OnDestroy()
        {
            if (MainClient.Instance != null)
            {
                MainClient.Instance.MessageReceived -= OnDataHandler;
            }
        }

        public void MoveCamera(Vector3 newPos, Quaternion newRot)
        {
            _targetPos = newPos;
            _targetRot = newRot;
            _oldPos = Camera.transform.position;
            _oldRot = Camera.transform.rotation;
            _distance = (newPos - _oldPos).magnitude;
            _angle = Quaternion.Angle(_oldRot, newRot);
            _initializationTime = Time.time;
        }

        private void Update()
        {
            // Move Camera
            if (Camera.transform.position != _targetPos || Camera.transform.rotation != _targetRot)
            {
                var rotationSpeed = _angle / (_distance / _cameraSpeed);
                _currentPos = (Time.time - _initializationTime) * _cameraSpeed / _distance;
                _currentRot = (Time.time - _initializationTime) * rotationSpeed / _angle;
                Camera.transform.position = Vector3.Lerp(_oldPos, _targetPos, _currentPos);
                Camera.transform.rotation = Quaternion.Slerp(_oldRot, _targetRot, _currentRot);
            }
        }

        private static void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                if (message.Tag != LoginTags.LogoutSuccess)
                    return;

                RoomManager.Instance.CurrentRoom = null;
                RoomManager.Instance.LeaveRoom();
                SceneManager.LoadScene("Login");
            }
        }

        private void StartGame()
        {
            gameObject.SetActive(false);
            ChatWindowManager.DeactivatePanels();
            switch (RoomManager.Instance.CurrentRoom.GameType)
            {
                case GameType.Arena:
                    MoveCamera(CameraPosSLA, CameraRotSLA);
                    break;
                case GameType.RunlingRun:
                    MoveCamera(CameraPosRLR, CameraRotRLR);
                    break;
                default:
                    Debug.Log("No room set!");
                    break;
            }
        }

        public void CloseAll()
        {
            SoloMenu.gameObject.SetActive(false);
            MultiplayerMenu.gameObject.SetActive(false);
            SLAMenu.gameObject.SetActive(false);
            RLRMenu.gameObject.SetActive(false);
            HighScoreMenuSLA.gameObject.SetActive(false);
            HighScoreMenuRLR.gameObject.SetActive(false);
            ChatWindowManager.DeactivatePanels();
        }
    }
}