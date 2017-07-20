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


        private void Awake()
        {
            
        }

        private Manticore(CharacterDto characterDto): base(characterDto)
        {

        }

        public override void Initizalize(CharacterDto character)
        {
            InitiazlizeBase(character);
            // something about abilities or in awake
            AbilityFirst = new Boost(this);
            AbilitySecond = new Shield(this);
        }


    }
}
