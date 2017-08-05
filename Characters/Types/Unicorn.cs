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
            Ability1 = new Freeze(this);
            Ability2 = new GravityField(this);
        }


    }
}
