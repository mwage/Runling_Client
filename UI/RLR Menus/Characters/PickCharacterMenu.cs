using System;
using Launcher;
using Players.Camera;
using UnityEngine;
using System.Linq;
using Characters.Types;
using System.Collections.Generic;
using Characters;
using Characters.Repositories;
using UnityEngine.UI;

namespace UI.RLRMenus.Characters
{
    public class PickCharacterMenu : MonoBehaviour
    {
        public GameObject CreateCharacterMenu;

        [NonSerialized]
        public int Id; // id of picked slot, value 1-8

        private ICharacterRepository _characterRepository;
        private ToggleGroup _slotsToggleGroup;
        private List<Text> _charactersText;
        
        public void Awake()
        {
            _characterRepository = new PlayerPrefsCharacterRepository();
            _slotsToggleGroup = CharacterController.FindObjectOfType<ToggleGroup>();
        }

        public void OnEnable()
        {
            FullfilTogglesTexts(_characterRepository.GetAll());
        }

        public void Pick()
        {
            if (_characterRepository.Get(Id).Occupied)
            {
                gameObject.SetActive(false);
                //  TODO create character and start game
            }
            else
            {
                return; //didnt do nuffin
            }
        }

        public void PickCharacter()
        {
            var activeSlots = _slotsToggleGroup.ActiveToggles();
            Toggle activeSlot = activeSlots.SingleOrDefault();
            if (activeSlot == null) return; // again problem with 2nd call
            if (!activeSlot.IsActive()) return; // TODO: function is called 2 times, 1: when one toggle is activated (and its ok) and 2: (when last one is deactivated) - bad. its hotfix 

            if (!Int32.TryParse(activeSlot.name, out Id))
            {
            Debug.Log("picked character slot is wrong");
            }
            else
            {
                Debug.Log(String.Concat("Picked id:", Id.ToString()));
                if (!_characterRepository.Get(Id).Occupied)
                {
                    gameObject.SetActive(false);
                    CreateCharacterMenu.SetActive(true);

                }
            }
        }

        public void Delete()
        {
            _characterRepository.Remove(Id);
            _charactersText[Id-1].text = "Empty";
        }

        private void FullfilTogglesTexts(List<CharacterDto> characters)
        {
            _charactersText = new List<Text>();
            var slots = gameObject.transform.Find("Slots"); // get slots from unity
            foreach (Transform slot in slots.transform)
            {
                _charactersText.Add(slot.Find("Button").Find("ButtonText").GetComponent<Text>());
            }
            // fullfil toggles' slots with proprer text
            for (int i = 0; i < LevelingSystem.MaxCharactersAmount; i++)
            {
                _charactersText[i].text = characters[i].Occupied ? characters[i].Character : "Empty";
            }
        }

    }
}
