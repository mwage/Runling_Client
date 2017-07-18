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
        // ablitiy 1
        // ablitiy 2
        public AAbility AbilityFirst { get; protected set; }
        public AAbility AbilitySecond { get; protected set; }

        private Unicorn(CharacterDto characterDto): base(characterDto)
        {
            //AbilityFirst = a1;
            //AbilitySecond = a2;
        }

        public override void Initizalize(CharacterDto character)
        {
            InitiazlizeBase(character);
            // something about abilities or in awake
            //AbilityFirst = a1;
            //AbilitySecond = a2;
        }

        public static Unicorn Create(CharacterDto character)
        {
            return new Unicorn(character);
        }
    }
}
