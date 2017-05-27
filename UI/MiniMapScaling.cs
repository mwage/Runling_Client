using System.Collections;
using System.Collections.Generic;
using Launcher;
using UnityEngine;

namespace UI
{
    public class MiniMapScaling : MonoBehaviour
    {
        public Camera Camera;
        public float MapSize;


        private void Awake()
        {
            MapSize = 5;
            SetMiniMapSize(MapSize);
        }

        public void SetMiniMapSize(float mapSize)
        {
            var mapWidth = 1 / MapSize;
            var mapHeight = Screen.width / (MapSize * Screen.height);
            Camera.rect = new Rect(0, 0, mapWidth, mapHeight);
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
