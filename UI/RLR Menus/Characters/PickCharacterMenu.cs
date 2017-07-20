using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Repositories;
using Characters.Types;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RLR_Menus.Characters
{
    public class PickCharacterMenu : MonoBehaviour
    {
        public GameObject CreateCharacterMenu;
        public List<Toggle> Toggs;
        public Toggle ActiveSlot;

        public int? Id { get; private set; } // id of picked slot, value 1-8, if not set or respond to not valid character - 0

        [NonSerialized] public int PickedSlot;

        private ICharacterRepository _characterRepository;
        private ToggleGroup _slotsToggleGroup;
        private List<Text> _slotsText;
        private Transform _preview;

        public void Awake()
        {
            // TODO: set ID to last game picked id
            _characterRepository = new PlayerPrefsCharacterRepository();
            _characterRepository.Remove(0); // remove 0-id character
            _slotsToggleGroup = CharacterController.FindObjectOfType<ToggleGroup>();

            _slotsText = new List<Text>();
            var slots = gameObject.transform.Find("Slots"); // get slots from unity
            foreach (Transform slot in slots.transform)
            {
                _slotsText.Add(slot.Find("Button").Find("ButtonText").GetComponent<Text>());
            }

            _preview = gameObject.transform.Find("Preview");
            if (Id != null)
            {
                if (!_characterRepository.Get((int)Id).Occupied) Id = null; // in case if someone delete his character in previous game, or other unpredictable way
            }
            
        }

        public void OnEnable()
        {
            FullfilTogglesTexts(_characterRepository.GetAll());
        }

        //public void Pick()
        //{
        //    if (_characterRepository.Get(Id).Occupied)
        //    {
        //        gameObject.SetActive(false);
        //        //  TODO create character and start game
        //    }
        //    else
        //    {
        //        return; //didnt do nuffin
        //    }
        //}

        public void PickSlot()
        {
            var activeSlots = _slotsToggleGroup.ActiveToggles();
            // TODO: function is called 2 times, 1: when one toggle is activated (and its ok) and 2: (when last one is deactivated) - bad. its hotfix 
            if (activeSlots.SingleOrDefault() != null && activeSlots.SingleOrDefault().IsActive())
            {
                ActiveSlot = activeSlots.SingleOrDefault();
            }
            else
            {
                return;
            }
            

            if (int.TryParse(ActiveSlot.name, out PickedSlot))
            {
                Debug.Log(String.Concat("Picked slot:", PickedSlot.ToString()));
                var pickedCharacter = _characterRepository.Get(PickedSlot);
                
                if (!pickedCharacter.Occupied) // enable CreateCharacterMenu
                {
                    SetId(null);
                    gameObject.SetActive(false);
                    CreateCharacterMenu.SetActive(true);
                }
                else // fullfil preview
                {
                    SetStatsValues(_preview.Find("StatsValues").GetComponent<Text>(), pickedCharacter);
                    // TODO setminiature
                    Id = PickedSlot;

                }
            }
        }

        public void Delete()
        {
            if (Id == null) return;
            if (PickedSlot != Id) return;
            _characterRepository.Remove(PickedSlot);
            _slotsText[PickedSlot - 1].text = "Empty";
            SetId(null);
            UnselectAllSlots();
        }

        private void FullfilTogglesTexts(List<CharacterDto> characters)
        {
            for (int i = 0; i < LevelingSystem.MaxCharactersAmount; i++)
            {
                _slotsText[i].text = characters[i].Occupied ? characters[i].Character : "Empty";
            }
        }

        private void SetStatsValues(Text statsValues, CharacterDto character)
        { //TODO maybe change speedpoints to speed?
            statsValues.text = String.Format("{0}\n{1}\n{2}\n{3}\n{4}/{5}", character.Level, character.SpeedPoints,
                character.RegenPoints, character.EnergyPoints, character.AbilityFirstLevel,
                character.AbilitySecondLevel);
        }

        private void SetMiniature()
        {
            
        }

        public void SetId(int? value)
        {
            Id = value;
        }

        public void UnselectAllSlots()
        {
            _slotsToggleGroup.SetAllTogglesOff();
        }

        public CharacterDto GetCharacterDto()
        {
            if (Id == null)
            {
                return PickFirstValidCharacterOrMakeNewOne();
            }
            return _characterRepository.Get((int)Id);
        }

        public CharacterDto PickFirstValidCharacterOrMakeNewOne()
        {
            if (_characterRepository == null) _characterRepository = new PlayerPrefsCharacterRepository();
            for (int i = 1; i < LevelingSystem.MaxCharactersAmount; i++)
            {
                if (_characterRepository.Get(i).Occupied)
                {
                    return _characterRepository.Get(i);
                }
            }
            _characterRepository.Add(1, "Manticore");
            return _characterRepository.Get(1);

        }
    }
}
