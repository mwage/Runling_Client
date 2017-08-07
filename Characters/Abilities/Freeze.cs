using System.Collections;
using System.Collections.Generic;
using Characters.Types;
using Drones;
using Launcher;
using Players;
using TMPro;
using UnityEngine;

namespace Characters.Abilities
{
    /// <summary>
    /// Ability that freezes all drones nearby player for few seconds
    /// </summary>
    public class Freeze : AAbility
    {
        private readonly PlayerManager _playerManager;       // TODO: Animation

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

        public float Range
        {
            get { return 150F; }
        }

        public Freeze(ACharacter character, PlayerManager playerManager)
        {
            Name = "Freeze";
            Level = character.Ability1Level;
            EnergyDrainPerSecond = 0;
            IsActive = false;
            _playerManager = playerManager;
            SetLoaded();
        }

        public List<Transform> DronesInRange = new List<Transform>();

        public override IEnumerator Enable(ACharacter character)
        {
            if (!IsLoaded)
            {
                Debug.Log(string.Format("colldown on: {0}", TimeToRenew));
                yield return null;
            }
            if (character.UseEnergy(EnergyCost)) // characterd had enough energy and used it
            {
                FindDronesInRange(GetDronesTransform());
                FreezeDrones();
                TimeToRenew = Cooldown;
                IsLoaded = false;
            }
        }

        public override void Disable(ACharacter character)
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
            var playerTransform = _playerManager.transform;
            foreach (var drone in droneTransforms)
            {
                if (IsInRange(playerTransform, drone, Range))
                {
                    DronesInRange.Add(drone);
                }
            }
        }

        private bool IsInRange(Transform player, Transform drone, float range)
        {
            if ( (drone.position - player.position).sqrMagnitude <= range )
            {
                return true;
            }
            return false;
        }
    }
}