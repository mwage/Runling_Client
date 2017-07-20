using UnityEngine;
using Characters.Types;
using Characters.Types.Features;
using Launcher;

namespace Characters.Abilities
{
    public class Shield : AAbility
    {
        private GameObject ShieldModel;

        public override int Cooldown
        {
            get { return 10 - Level; }
        }

        public override int EnergyCost
        {
            get { return 10 - Level; }
        }

        public Shield(ACharacter character)
        {
            Name = "Boost";
            Level = character.AbilitySecondLevel;
            EnergyDrainPerSecond = 0;
            IsActive = false;
            ShieldModel = GameControl.PlayerState.Player.transform.Find("Shield").gameObject;
        }




        public override void Enable(ACharacter character)
        {
            if (IsActive) return;
            if (!IsLoaded) return;
            if (character.UseEnergy(EnergyCost)) // characterd had enough energy and used it
            {
                ActivateShieldModel();
                TimeToRenew = Cooldown;
                IsLoaded = false;
                IsActive = true;
                character.Energy.EnergyDrainPerSec = 0F;
                character.Energy.RegenStatus = RegenStatus.Blocked;
            }
        }

        public override void Disable(ACharacter character)
        {
            if (!IsActive) return;
            DeactivateShieldModel();
            character.Energy.RegenStatus = RegenStatus.Regen;
            IsActive = false;
        }




        private void ActivateShieldModel()
        {
            ShieldModel.SetActive(true);
        }

        private void DeactivateShieldModel()
        {
            ShieldModel.SetActive(false);
        }






    }
}