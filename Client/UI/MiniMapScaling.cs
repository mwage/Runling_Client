using Client.Scripts.Launcher;
using UnityEngine;

namespace Client.Scripts.UI
{
    public class MiniMapScaling : MonoBehaviour
    {
        public Camera Camera;
        private const float MapSize = 5;

        private void Awake()
        {
            SetMiniMapSize(MapSize);
        }

        public void SetMiniMapSize(float mapSize)
        {
            const float mapWidth = 1 / MapSize;
            var mapHeight = Screen.width / (MapSize * Screen.height);
            Camera.rect = new Rect(1-mapWidth, 0, mapWidth, mapHeight);
        }

        private void Update()
        {
            if (Camera.gameObject.activeSelf == GameControl.Settings.HideMiniMap)
            {
                Camera.gameObject.SetActive(!GameControl.Settings.HideMiniMap);
            }
        }
    }
}
