using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Characters.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Types
{
    public class Unicorn : ACharacter
    {
        private Unicorn(CharacterDto characterDto): base(characterDto)
        {
        }

        public override void Initialize(CharacterDto character)
        {
            InitializeBase(character);
            // something about abilities or in awake
            //Ability1 = a1;
            //Ability2 = a2;
        }

        protected override void ActivateOrDeactivateAbility1()
        {
            throw new NotImplementedException();
        }

        protected override void ActivateOrDeactivateAbility2()
        {
            throw new NotImplementedException();
        }

        public static Unicorn Create(CharacterDto character)
        {
            return new Unicorn(character);
        }
    }
}
