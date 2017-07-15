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
            _baseSpeed = 7;
            _regenPerSecondRatio = 0.8F;
            _speedPointRatio = 0.09F;
            //AbilityFirst = a1;
            //AbilitySecond = a2;
        }

        public override void Initizalize(CharacterDto character)
        {
            InitiazlizeBase(character);
            // something about abilities or in awake
            _baseSpeed = 10;
            _regenPerSecondRatio = 0.3F;
            _speedPointRatio = 0.05F;
            //AbilityFirst = a1;
            //AbilitySecond = a2;
        }

        public static Unicorn Create(CharacterDto character)
        {
            return new Unicorn(character);
        }
    }
}
