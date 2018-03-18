using Client.Scripts.Launcher;
using Game.Scripts.Drones;
using Game.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Characters.Abilities
{
    [CreateAssetMenu(fileName = "New Freeze Ability", menuName = "Ability/Freeze")]
    public class Freeze : AAbility
    {
        public float BaseDuration = 2;
        public float DurationChangePerLevel = 1;
        public float Range = 6;

        public override bool IsToggle => false;
        protected float Duration => BaseDuration + DurationChangePerLevel * (Level - 1);

        public List<Transform> DronesInRange = new List<Transform>();

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
                drone.gameObject.GetComponent<DroneManager>().Freeze(Duration);
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