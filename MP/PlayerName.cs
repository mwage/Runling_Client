using UnityEngine;
using UnityEngine.UI;

namespace MP
{
    public class PlayerName : MonoBehaviour
    {
        private void Start()
        {

            var playerName = "";
            var inputField = GetComponent<InputField>();

            if (PlayerPrefs.HasKey("PlayerName"))
            {
                playerName = PlayerPrefs.GetString("PlayerName");
                inputField.text = playerName;
            }

            PhotonNetwork.playerName = playerName;
        }

        public void SetPlayerName(string value)
        {
            PhotonNetwork.playerName = value;
            PlayerPrefs.SetString("PlayerName", value);
        }
    }
}
