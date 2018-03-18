using Client.Scripts.Launcher;
using Game.Scripts.Drones;
using Game.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Characters.Abilities
{
    [CreateAssetMenu(fileName = "New Gravity Field Ability", menuName = "Ability/Gravity Field")]
    public class GravityField : AAbility
    {
        public float EnergyDrainPerSecond = 1;
        public float Range = 150;
        public float Magnitude = 0.2f;

        public override bool IsToggle => true;

        public List<Transform> DronesInRangeNew = new List<Transform>(); //TODO: reserve memory if performance will be bad
        public List<Transform> DronesInRangeOld = new List<Transform>();

        public override IEnumerator Enable()
        {
            if (!IsUsable)
            {
                Debug.Log("On Cooldown: " + TimeToRenew);
                yield break;
            }

            // Check if character has enough energy and use it
            if (CharacterManager.Energy.UseEnergy(ActivationCost))
            {
                IsUsable = false;
                IsActive = true;
                TimeToRenew = Cooldown;
                CharacterManager.Energy.EnableEnergyDrain(EnergyDrainPerSecond);

                while (IsActive)
                {
                    DronesInRangeNew = FindDronesInRange(GetDronesTransforms());
                    ActivateOnNewDronesInRange();
                    DeactivateOnDronesOutOfRange();
                    DronesInRangeOld = DronesInRangeNew;
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        public override void Disable()
        {
            IsActive = false;
            CharacterManager.Energy.DisableEnergyDrain();
            foreach (var drone in DronesInRangeNew)
            {
                drone.gameObject.GetComponent<DroneManager>().DisableSlowdown();
            }
        }

        private static List<Transform> GetDronesTransforms()
        {
            return GameControl.GameState.DronesParent.transform.Cast<Transform>().ToList();
        }

        private List<Transform> FindDronesInRange(List<Transform> droneTransforms)
        {
            return droneTransforms.Where(drone => IsInRange(PlayerManager.transform, drone, Range)).ToList();
        }

        private void ActivateOnNewDronesInRange()
        {
            foreach (var drone in DronesInRangeNew)
            {
                if (!DronesInRangeOld.Contains(drone))
                {
                    if (!drone.gameObject.GetComponent<DroneManager>().IsSlowed)
                    {
                        drone.gameObject.GetComponent<DroneManager>().EnableSlowdown(Magnitude); 
                        // TODO: MP: check if its already slowed down and pick the strongest slow (or maybe not) -now the first player's slow is active
                    }
                    else
                    {
                        DronesInRangeNew.Remove(drone); // removes drone from list as that list is later assigned to DronesInRangeOld, which corresponds to UnSlowing drones that are out of range. -> Player dont unslow not his drones.
                    }
                }
            }
        }

        private void DeactivateOnDronesOutOfRange()
        {
            foreach (var drone in DronesInRangeOld)
            {
                if (!DronesInRangeNew.Contains(drone))
                {
                    drone.gameObject.GetComponent<DroneManager>().DisableSlowdown();
                }
            }
        }

        private static bool IsInRange(Transform player, Transform drone, float range)
        {
            return (drone.position - player.position).sqrMagnitude <= range;
        }
    }
}