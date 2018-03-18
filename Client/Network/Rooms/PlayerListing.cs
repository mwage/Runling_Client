using Game.Scripts.GameSettings;
using Game.Scripts.Network.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Network.Rooms
{
    public class PlayerListing : MonoBehaviour
    {
        [SerializeField] private Text _playerName;
        [SerializeField] private Image _colorPreview;
        public Player Player { get; private set; }

        public void Initialize(Player player)
        {
            Player = player;
            _playerName.text = player.IsHost ? player.Name + " (Host)" : player.Name;
            SetColor();
        }

        private void SetColor()
        {
            switch (Player.Color)
            {
                case PlayerColor.Green:
                    _colorPreview.color = Color.green;
                    break;
                case PlayerColor.Blue:
                    _colorPreview.color = Color.blue;
                    break;
                case PlayerColor.Red:
                    _colorPreview.color = Color.red;
                    break;
                default:
                    Debug.Log("Invalid Color");
                    break;
            }
        }
    }
}