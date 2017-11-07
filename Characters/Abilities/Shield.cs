using Characters.Types;
using Characters.Types.Features;
using Players;
using System.Collections;
using UnityEngine;

namespace Characters.Abilities
{
    public class Shield : AAbility
    {
        public override int Cooldown => 10 - Level;
        public override int EnergyCost => 10 - Level;
        public float DurationTime => Level + 5;

        public Shield(ACharacter character, PlayerManager playerManager)
        {
            Name = "Shield";
            Level = character.Ability2Level;
            EnergyDrainPerSecond = 0;
            IsActive = false;
            PlayerManager = playerManager;
            Character = character;
            SetLoaded();
        }


        public override IEnumerator Enable()
        {
            if (IsActive)
                yield break;

            if (!IsUsable)
            {
                Debug.Log("On Cooldown: " + TimeToRenew);
                yield break;
            }

            // Check if character has enough energy and use it
            if (Character.Energy.UseEnergy(EnergyCost)) 
            {
                PlayerManager.Shield.SetActive(true);
                PlayerManager.IsInvulnerable = true;
                TimeToRenew = Cooldown;
                IsUsable = false;
                IsActive = true;
                Character.Energy.EnergyDrainPerSec = 0F;
                if (Character.Energy.RegenStatus == RegenStatus.Regen)
                {
                    Character.Energy.RegenStatus = RegenStatus.Blocked;
                }

                yield return new WaitForSeconds(DurationTime);
                Disable();

            }
        }

        public override void Disable()
        {
            if (!IsActive)
                return;

            PlayerManager.Shield.SetActive(false);
            PlayerManager.IsInvulnerable = false;
            Character.Energy.RegenStatus = RegenStatus.Regen;
            IsActive = false;
        }
    }
}