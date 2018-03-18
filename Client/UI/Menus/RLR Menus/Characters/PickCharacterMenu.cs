using Game.Scripts.Characters;
using Game.Scripts.Characters.CharacterRepositories;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.UI.Menus.RLR_Menus.Characters
{
    public class PickCharacterMenu : MonoBehaviour
    {
        public GameObject CreateCharacterMenu;
        public int PickedSlot { get; set; }
        public ICharacterRepository CharacterRepository { get; private set; }

        private ToggleGroup _slotsToggleGroup;
        private readonly List<Text> _slotsText = new List<Text>();
        private Transform _preview;
        private Toggle _activeToggle;

        public void Awake()
        {
            // TODO: When playing online: access from DB instead
            CharacterRepository = new CharacterRepositoryPlayerPrefs();
            
            // Get all 8 slots and text references
            var slots = gameObject.transform.Find("Slots"); // get slots from unity
            _slotsToggleGroup = slots.GetComponent<ToggleGroup>();

            foreach (Transform slot in slots.transform)
            {
                _slotsText.Add(slot.Find("Button").Find("ButtonText").GetComponent<Text>());
            }

            _preview = gameObject.transform.Find("Preview");
        }

        public void OnEnable()
        {
            SetTogglesTexts(CharacterRepository.GetAll());
        }

        public void PickSlot()
        {
            var activeSlot = _slotsToggleGroup.ActiveToggles().FirstOrDefault(t => t.IsActive());
            if (activeSlot == null)
            {
                Debug.Log("No character selected!");
                return;
            }

            int slot;
            if (!int.TryParse(activeSlot.name, out slot) || slot == PickedSlot)
                return;

            PickedSlot = slot;
            _activeToggle = activeSlot;
            Debug.Log("Picked slot: " + slot);
            var pickedCharacter = CharacterRepository.Get(PickedSlot);

            // If character doesn't exist, enable CreateCharacterMenu
            if (!pickedCharacter.Occupied) 
            {
                gameObject.SetActive(false);
                CreateCharacterMenu.SetActive(true);
            }
            else
            {
                SetStats(_preview.Find("StatsValues").GetComponent<Text>(), pickedCharacter);
            }
        }

        public void Delete()
        {
            if (PickedSlot == 0)
            {
                Debug.Log("Select the character you want to delete.");
                return;
            }

            CharacterRepository.Remove(PickedSlot);
            _slotsText[PickedSlot - 1].text = "Empty";
            PickedSlot = 0;
            UnselectAllSlots();
        }

        private void SetTogglesTexts(List<CharacterDto> characters)
        {
            for (var i = 0; i < LevelingSystem.MaxCharactersAmount; i++)
            {
                _slotsText[i].text = characters[i].Occupied ? characters[i].Name : "Empty";
            }
        }

        private static void SetStats(Text statsValues, CharacterDto character)
        {
            statsValues.text =
                $"{character.Level}\n{character.SpeedPoints}\n{character.RegenPoints}\n{character.EnergyPoints}\n{character.AbilityLevels[0]}/{character.AbilityLevels[1]}";
        }

        public void UnselectAllSlots()
        {
            _slotsToggleGroup.SetAllTogglesOff();
            _activeToggle.isOn = false;
        }

        public CharacterDto GetCharacterDto()
        {
            return PickedSlot == 0 ? PickFirstValidCharacterOrMakeNewOne() : CharacterRepository.Get(PickedSlot);
        }

        public CharacterDto PickFirstValidCharacterOrMakeNewOne()
        {
            // Temporary so the Test Button works!
            if (CharacterRepository == null)
            {
                CharacterRepository = new CharacterRepositoryPlayerPrefs();
            }
            for (var i = 1; i < LevelingSystem.MaxCharactersAmount; i++)
            {
                if (CharacterRepository.Get(i).Occupied)
                {
                    return CharacterRepository.Get(i);
                }
            }
            CharacterRepository.Add(1, CreateCharacterMenu.GetComponent<CreateCharacterMenu>().CharacterStorage.AvailableCharacters[0]);
            return CharacterRepository.Get(1);
        }
    }
}
