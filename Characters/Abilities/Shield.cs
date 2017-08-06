using System.Collections;
using UnityEngine;
using Characters.Types;
using Characters.Types.Features;
using Players;

namespace Characters.Abilities
{
    public class Shield : AAbility
    {
        private readonly PlayerManager _playerManager;

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
            get { return Level + 5F; }
        }

        public Shield(ACharacter character, PlayerManager playerManager)
        {
            Name = "Boost";
            Level = character.Ability2Level;
            EnergyDrainPerSecond = 0;
            IsActive = false;
            _playerManager = playerManager;
            SetLoaded();
        }


        public override IEnumerator Enable(ACharacter character)
        {
            if (IsActive) yield return null;
            if (!IsLoaded)
            {
                Debug.Log(string.Format("colldown on: {0}", TimeToRenew));
                yield return null;
            }
            if (character.UseEnergy(EnergyCost)) // characterd had enough energy and used it
            {
                _playerManager.Shield.SetActive(true);
                _playerManager.IsInvulnerable = true;
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
            _playerManager.Shield.SetActive(false);
            _playerManager.IsInvulnerable = false;
            character.Energy.RegenStatus = RegenStatus.Regen;
            IsActive = false;
        }
    }
}