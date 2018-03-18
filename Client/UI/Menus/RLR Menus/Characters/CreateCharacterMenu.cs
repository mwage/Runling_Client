using System.Collections.Generic;
using Game.Scripts.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.UI.Menus.RLR_Menus.Characters
{
    public class CreateCharacterMenu : MonoBehaviour
    {
        public PickCharacterMenu PickCharacterMenu;
        public CharacterStorage CharacterStorage;
        public GameObject SelectCharacterButtonPrefab;
        public Transform CharacterButtons;
        public Text CharacterName;
        public Image PreviewImage;
        public Text CharacterDescription;
        public Text FirstAbilityText;
        public Text SecondAbilityText;

        private readonly List<SelectCharacterButton> _buttons = new List<SelectCharacterButton>();
        private Character _selectedCharacter;

        private void Awake()
        {
            foreach (var character in CharacterStorage.AvailableCharacters)
            {
                var button = Instantiate(SelectCharacterButtonPrefab, CharacterButtons).GetComponent<SelectCharacterButton>();
                button.Initialize(this, character);
                _buttons.Add(button);
            }
            _buttons[0].Toggle.isOn = true;
        }

        public void SelectCharacter(Character character)
        {
            _selectedCharacter = character;
            CharacterName.text = character.Name;
            CharacterDescription.text = character.Description;
            PreviewImage.sprite = character.PreviewImage;
            FirstAbilityText.text = "Ability 1: " + character.Abilities[0]?.Name + " - " + character.Abilities[0]?.Description;
            SecondAbilityText.text = "Ability 1: " + character.Abilities[1]?.Name + " - " + character.Abilities[1]?.Description;
        }

        public void Pick()
        {
            PickCharacterMenu.CharacterRepository.Add(PickCharacterMenu.PickedSlot, _selectedCharacter);
            PickCharacterMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            PickCharacterMenu.PickedSlot = 0;
            PickCharacterMenu.UnselectAllSlots();
            PickCharacterMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
