using System.Collections.Generic;
using Client.Scripts.IngameCamera;
using Client.Scripts.Launcher;
using Game.Scripts.Characters;
using Game.Scripts.Characters.Abilities;
using UnityEngine;

namespace Client.Scripts.PlayerInput.Buttons
{
    public class ButtonManager : MonoBehaviour
    {
        public GameObject AbilityButtonPrefab;
        public GameObject FollowButton;
        public Transform LayoutGroup;
        public CameraMovement CameraMovement;

        private readonly Dictionary<AbilityButton, AAbility> _abilityButtons = new Dictionary<AbilityButton, AAbility>();
        
        public void InitializeAbilityButtons(CharacterManager characterManager)
        {
            GetComponent<CharacterWindow>().Initialize(characterManager);

            foreach (var ability in characterManager.Character.Abilities)
            {
                var abilityButton = Instantiate(AbilityButtonPrefab, LayoutGroup).GetComponent<AbilityButton>();
                abilityButton.Initialize(ability, characterManager);
                _abilityButtons[abilityButton] = ability;
            }
        }

        public void LateUpdate()
        {
            foreach (var button in _abilityButtons.Keys)
            {
                button.SetProgress();
            }
            FollowButton.SetActive(GameControl.Settings.FollowEnabled);
        }

        public void ToggleFollow()
        {
            CameraMovement.ActivateOrDeactivateFollow();
        }
    }
}