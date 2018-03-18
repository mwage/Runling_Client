using System;
using Game.Scripts.Players;
using System.Collections;
using UnityEngine;

namespace Game.Scripts.Characters.Abilities
{
    public abstract class AAbility : ScriptableObject
    {
        public string Name = "Ability Name";
        public string Description = "Description";
        public GameObject AnimationPrefab;
        public Sprite Icon;
        public float BaseCooldown = 5;
        public int BaseActivationCost = 10;

        public bool IsActive { get; protected set; }
        public bool IsUsable { get; protected set; } = true;
        public float TimeToRenew { get; protected set; }
        public int Level => CharacterManager.Stats.AbilityLevels[Array.IndexOf(CharacterManager.Character.Abilities, this)];

        protected CharacterManager CharacterManager { get; set; }
        protected PlayerManager PlayerManager { get; set; }
        protected GameObject Animation { get; private set; }

        public abstract bool IsToggle { get; }
        public virtual float Cooldown => BaseCooldown;
        public virtual int ActivationCost => BaseActivationCost;

        public abstract IEnumerator Enable();
        public abstract void Disable();

        public void Initialize(PlayerManager playerManager)
        {
            PlayerManager = playerManager;
            CharacterManager = playerManager.CharacterManager;
            Animation = Instantiate(AnimationPrefab, playerManager.transform);
            Animation.SetActive(false);
        }

        public void RefreshCooldown()
        {
            if (IsUsable)
                return;

            if (TimeToRenew <= 0)
            {
                IsUsable = true;
                TimeToRenew = 0;
            }
            else
            {
                TimeToRenew -= Time.deltaTime;
            }
        }

        protected void SetLoaded()
        {
            TimeToRenew = 0F;
            IsUsable = true;
        }
        
        public float GetLoadingProgress()
        {
            if (IsUsable)
                return 1;

            return 1 - TimeToRenew / Cooldown;
        }
    }
}
