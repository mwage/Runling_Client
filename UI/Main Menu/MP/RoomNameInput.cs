using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.MP
{
    public class RoomNameInput : MonoBehaviour
    {
        public string CustomRoomName { get; private set; }

        private void Start()
        {
            var inputField = GetComponent<InputField>();

            if (PlayerPrefs.HasKey("RoomName"))
            {
                CustomRoomName = PlayerPrefs.GetString("RoomName");
                inputField.text = CustomRoomName;
            }
        }

        public void SetCustomRoomName(string value)
        {
            CustomRoomName = value;
            PlayerPrefs.SetString("RoomName", value);
            Debug.Log(CustomRoomName);
        }
    }
}
