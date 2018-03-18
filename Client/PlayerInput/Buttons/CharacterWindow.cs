using Client.Scripts.Launcher;
using Game.Scripts.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.PlayerInput.Buttons
{
    public class CharacterWindow : MonoBehaviour
    {
        public Text CharacterName;
        public Text RegenPoints;
        public Text EnergyPoints;
        public Text SpeedPoints;
        public Text UnspentPoints;
        public Image FirstAbilityIcon;
        public Image SecondAbilityIcon;
        public Text FirstAbilityLevel;
        public Text SecondAbilityLevel;
        public GameObject CharacterWindowObject;

        private CharacterManager _characterManager;
        
        public void Initialize(CharacterManager characterManager)
        {
            _characterManager = characterManager;
            CharacterName.text = characterManager.Character.Name;
            FirstAbilityIcon.sprite = characterManager.Character.Abilities[0]?.Icon;
            SecondAbilityIcon.sprite = characterManager.Character.Abilities[1]?.Icon;
        }

        private void Update()
        {
            RegenPoints.text = _characterManager?.Stats.RegenPoints.ToString();
            EnergyPoints.text = _characterManager?.Stats.EnergyPoints.ToString();
            SpeedPoints.text = _characterManager?.Stats.SpeedPoints.ToString();
            UnspentPoints.text = _characterManager?.Stats.UnspentPoints.ToString();
            FirstAbilityLevel.text = _characterManager?.Stats.AbilityLevels[0] + "/" + LevelingSystem.AbilityMaxLevel;
            SecondAbilityLevel.text = _characterManager?.Stats.AbilityLevels[1] + "/" + LevelingSystem.AbilityMaxLevel;
        }

        public void IncrementRegen()
        {
            if (GameControl.GameState.Solo)
            {
                _characterManager?.Stats.IncrementRegenPoints();
            }
            // TODO: MP: Send request to server
        }

        public void IncrementEnergy()
        {
            if (GameControl.GameState.Solo)
            {
                _characterManager?.Stats.IncrementEnergyPoints();
            }
            // TODO: MP: Send request to server
        }

        public void IncrementSpeed()
        {
            if (GameControl.GameState.Solo)
            {
                _characterManager?.Stats.IncrementSpeedPoints();
            }
            // TODO: MP: Send request to server
        }

        public void IncrementFirstAbility()
        {
            if (GameControl.GameState.Solo)
            {
                _characterManager?.Stats.IncrementAbilityLevel(0);
            }
        }

        public void IncrementSecondAbility()
        {
            if (GameControl.GameState.Solo)
            {
                _characterManager?.Stats.IncrementAbilityLevel(1);
            }
        }

        public void ToggleMenu()
        {
            CharacterWindowObject.SetActive(!CharacterWindowObject.activeSelf);
        }
    }
}
