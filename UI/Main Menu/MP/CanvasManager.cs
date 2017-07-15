using UnityEngine;

namespace UI.Main_Menu.MP
{
    public class CanvasManager : MonoBehaviour
    {
        public LobbyCanvas LobbyCanvas;

        public static CanvasManager Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}
