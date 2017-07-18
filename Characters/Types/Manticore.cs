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


    }
}
