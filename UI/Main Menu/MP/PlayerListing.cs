using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.MP
{
    public class PlayerListing : MonoBehaviour
    {
        [SerializeField] private Text _playerName;
        public PhotonPlayer PhotonPlayer { get; private set; }

        public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
        {
            PhotonPlayer = photonPlayer;
            _playerName.text = PhotonPlayer.NickName;
        }
    }
}