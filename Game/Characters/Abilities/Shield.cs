using System.Collections;
using UnityEngine;

namespace Game.Scripts.Characters.Abilities
{
    [CreateAssetMenu(fileName = "New Shield Ability", menuName = "Ability/Shield")]
    public class Shield : AAbility
    {
        public float CooldownReductionPerLevel = 2;
        public float Duration = 2;

        public override float Cooldown => BaseCooldown - (Level - 1) * CooldownReductionPerLevel;
        public override bool IsToggle => false;

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
            if (CharacterManager.Energy.UseEnergy(ActivationCost)) 
            {
                Animation.SetActive(true);
                PlayerManager.IsInvulnerable = true;
                TimeToRenew = Cooldown;
                IsUsable = false;
                IsActive = true;
                CharacterManager.Energy.BlockRegen();

                yield return new WaitForSeconds(Duration);
                Disable();
            }
        }

        public override void Disable()
        {
            if (!IsActive)
                return;

            Animation.SetActive(false);
            PlayerManager.IsInvulnerable = false;
            CharacterManager.Energy.ContinueRegen();
            IsActive = false;
        }
    }
}