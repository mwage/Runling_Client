﻿using UnityEngine;
using UnityEngine.UI;

namespace MP
{
    public class PlayerName : MonoBehaviour
    {
        private InputField _inputField;

        private void Start()
        {

            var playerName = "";
            _inputField = GetComponent<InputField>();

            if (PlayerPrefs.HasKey("PlayerName"))
            {
                playerName = PlayerPrefs.GetString("PlayerName");
                _inputField.text = playerName;
            }

            PhotonNetwork.playerName = playerName;
        }

        public void SetPlayerName(string value)
        {
            _inputField.text = value;
            PhotonNetwork.playerName = value;
            PlayerPrefs.SetString("PlayerName", value);
        }
    }
}