using System.Collections;
using System.Collections.Generic;
using Characters.Types;
using Characters.Types.Features;
using Drones;
using Launcher;
using Players;
using UnityEngine;

namespace Characters.Abilities
{
    /// <summary>
    /// Ability that slows down all drones nearby player while active
    /// </summary>
    public class GravityField : AAbility
    {
        private PlayerManager _playerManager;       // TODO: animation

        public override int Cooldown
        {
            get { return 10 - Level; }
        }

        public override int EnergyCost
        {
            get { return 1 - Level; }
        }

        public float Range
        {
            get { return 150F; }
        }

        public float Percentage
        {
            get { return 0.8F; }
        }

        public GravityField(ACharacter character, PlayerManager playerManager)
        {

            Name = "GravityField";
            Level = character.Ability2Level;
            EnergyDrainPerSecond = 1;
            IsActive = false;
            _playerManager = playerManager;
            SetLoaded();
        }

        public List<Transform> DronesInRangeNew = new List<Transform>(); //TODO: reserve memory if performance will be bad
        public List<Transform> DronesInRangeOld = new List<Transform>();

        public override IEnumerator Enable(ACharacter character)
        {
            if (!IsLoaded)
            {
                Debug.Log(string.Format("colldown on: {0}", TimeToRenew));
                yield return null;
            }
            if (character.UseEnergy(EnergyCost)) // characterd had enough energy and used it
            {
                IsLoaded = false;
                IsActive = true;
                TimeToRenew = Cooldown;
                character.Energy.EnergyDrainPerSec = EnergyDrainPerSecond;
                character.Energy.RegenStatus = RegenStatus.Drain;

                while (IsActive)
                {
                    DronesInRangeNew = FindDronesInRange(GetDronesTransforms());
                    ActivateOnNewDronesInRange();
                    DeactivateOnDronesOutOfRange();
                    DronesInRangeOld = DronesInRangeNew;
                    yield return new WaitForSeconds(0.01F);
                }
            }
        }

        public override void Disable(ACharacter character)
        {
            IsActive = false;
            character.Energy.RegenStatus = RegenStatus.Regen;
            foreach (var drone in DronesInRangeNew)
            {
                drone.gameObject.GetComponent<DroneManager>().DisableSlowdown();
            }
        }

        private List<Transform> GetDronesTransforms()
        {
            List<Transform> drones = new List<Transform>();
            foreach (Transform drone in GameControl.GameState.DronesParent.transform)
            {
                drones.Add(drone);
            }
            return drones;
        }

        private List<Transform> FindDronesInRange(List<Transform> droneTransforms)
        {
            List<Transform> drones = new List<Transform>();
            foreach (var drone in droneTransforms)
            {
                if (IsInRange(_playerManager.transform, drone, Range))
                {
                    drones.Add(drone);
                }
            }
            return drones;
        }

        private void ActivateOnNewDronesInRange()
        {
            foreach (Transform drone in DronesInRangeNew)
            {
                if (!DronesInRangeOld.Contains(drone))
                {
                    if (!drone.gameObject.GetComponent<DroneManager>().IsSlowed)
                    {
                        drone.gameObject.GetComponent<DroneManager>().EnableSlowdown(Percentage); 
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
            foreach (Transform drone in DronesInRangeOld)
            {
                if (!DronesInRangeNew.Contains(drone))
                {
                    drone.gameObject.GetComponent<DroneManager>().DisableSlowdown();
                }
            }
        }

        private bool IsInRange(Transform player, Transform drone, float range)
        {
            return (drone.position - player.position).sqrMagnitude <= range;
        }
    }
}