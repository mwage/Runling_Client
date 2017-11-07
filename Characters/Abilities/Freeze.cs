using Characters.Types;
using Drones;
using Launcher;
using Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Abilities
{
    /// <summary>
    /// Ability that freezes all drones nearby player for few seconds
    /// </summary>
    public class Freeze : AAbility
    {
        public override int Cooldown => 10 - Level;
        public override int EnergyCost => 10 - Level;
        public float DurationTime => Level + 3;
        public float Range => 150;

        public Freeze(ACharacter character, PlayerManager playerManager)
        {
            Name = "Freeze";
            Level = character.Ability1Level;
            EnergyDrainPerSecond = 0;
            IsActive = false;
            PlayerManager = playerManager;
            Character = character;
            SetLoaded();
        }

        public List<Transform> DronesInRange = new List<Transform>();

        public override IEnumerator Enable()
        {
            if (!IsUsable)
            {
                Debug.Log("On Cooldown: " + TimeToRenew);
                yield break;
            }
            
            // Check if character has enough energy and use it
            if (Character.Energy.UseEnergy(EnergyCost))
            {
                FindDronesInRange(GetDronesTransform());
                FreezeDrones();
                TimeToRenew = Cooldown;
                IsUsable = false;
            }
        }

        public override void Disable()
        {

        }

        private void FreezeDrones()
        {
            foreach (var drone in DronesInRange)
            {
                drone.gameObject.GetComponent<DroneManager>().Freeze(DurationTime);
            }
        }

        private List<Transform> GetDronesTransform()
        {
            List<Transform> drones = new List<Transform>();
            foreach (Transform drone in GameControl.GameState.DronesParent.transform)
            {
                drones.Add(drone);
            }
            return drones;
        }

        private void FindDronesInRange(List<Transform> droneTransforms)
        {
            DronesInRange.Clear();
            var playerTransform = PlayerManager.transform;
            foreach (var drone in droneTransforms)
            {
                if (IsInRange(playerTransform, drone, Range))
                {
                    DronesInRange.Add(drone);
                }
            }
        }

        private static bool IsInRange(Transform player, Transform drone, float range)
        {
            if ( (drone.position - player.position).sqrMagnitude <= range )
            {
                return true;
            }
            return false;
        }
    }
}