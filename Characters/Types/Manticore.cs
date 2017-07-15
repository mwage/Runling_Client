using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Characters.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Types
{
    public class Manticore : ACharacter
    {
        // ablitiy 1
        // ablitiy 2
        public AAbility AbilityFirst { get; protected set; }
        public AAbility AbilitySecond { get; protected set; }

        private void Awake()
        {
            
        }

        private Manticore(CharacterDto characterDto): base(characterDto)
        {
            _baseSpeed = 10;
            _regenPerSecondRatio = 0.3F;
            _speedPointRatio = 0.05F;
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


    }
}
