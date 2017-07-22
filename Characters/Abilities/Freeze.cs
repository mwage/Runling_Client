using System.Collections;
using System.Collections.Generic;
using Characters.Types;
using Launcher;
using UnityEngine;

namespace Characters.Abilities
{
    /// <summary>
    /// Ability that freezes all drones nearby player for few seconds
    /// </summary>
    public class Freeze : AAbility
    {
    // private GameObject SOMEANIMATION TODO;

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

        public override IEnumerator Enable(ACharacter character)
        {
            throw new System.NotImplementedException();
        }

        public override void Disable(ACharacter character)
        {
            throw new System.NotImplementedException();
        }

        private List<Transform> GetDronesTransform()
        {
            List<Transform> drones = new List<Transform>();
            foreach (Transform drone in GameControl.MapState.DronesParent.transform)
            {
                drones.Add(drone);
            }
            return drones;
        }
    }
}