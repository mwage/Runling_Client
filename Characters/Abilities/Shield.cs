using System.Collections;
using System.Collections.Generic;
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

        public float DurationTime
        {
            get { return Level + 3F; }
        }

        public Shield(ACharacter character)
        {
            Name = "Boost";
            Level = character.AbilitySecondLevel;
            EnergyDrainPerSecond = 0;
            IsActive = false;
            SetLoaded();
        }

        private void Initialize()
        {
            if (ShieldModel == null)
            {
                ShieldModel = GameControl.PlayerState.Player.transform.Find("Shield").gameObject;
            }
        }



        public override IEnumerator Enable(ACharacter character)
        {
            if (IsActive) yield return null;
            if (!IsLoaded)
            {
                Debug.Log(string.Format("colldown on: {0}", Cooldown));
                yield return null;
            }
            if (character.UseEnergy(EnergyCost)) // characterd had enough energy and used it
            {
                ActivateShieldModel();
                GameControl.PlayerState.IsInvulnerable = true;
                TimeToRenew = Cooldown;
                IsLoaded = false;
                IsActive = true;
                character.Energy.EnergyDrainPerSec = 0F;
                if (character.Energy.RegenStatus == RegenStatus.Regen)
                {
                    character.Energy.RegenStatus = RegenStatus.Blocked;
                }
                yield return new WaitForSeconds(DurationTime);
                Disable(character);

            }
        }

        public override void Disable(ACharacter character)
        {
            if (!IsActive) return;
            DeactivateShieldModel();
            GameControl.PlayerState.IsInvulnerable = false;
            character.Energy.RegenStatus = RegenStatus.Regen;
            IsActive = false;
        }




        private void ActivateShieldModel()
        {
            Initialize();
            ShieldModel.SetActive(true);
        }

        private void DeactivateShieldModel()
        {
            ShieldModel.SetActive(false);
        }

        IEnumerator Wait3s()
        {
            yield return new WaitForSeconds(3);
        }







    }
}