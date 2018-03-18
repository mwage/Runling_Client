using Game.Scripts.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.UI.Menus.RLR_Menus.Characters
{
    public class SelectCharacterButton : MonoBehaviour
    {
        public Text ButtonText;

        private Character _character;
        private CreateCharacterMenu _createCharacterMenu;
        public Toggle Toggle { get; private set; }

        private void Awake()
        {
            Toggle = GetComponent<Toggle>();
            Toggle.group = transform.parent.GetComponent<ToggleGroup>();
        }

        public void Initialize(CreateCharacterMenu createCharacterMenu, Character character)
        {
            _createCharacterMenu = createCharacterMenu;
            _character = character;
            ButtonText.text = character.Name;
        }

        public void Select()
        {
            if (Toggle.isOn)
            {
                _createCharacterMenu.SelectCharacter(_character);
            }
        }
    }
}