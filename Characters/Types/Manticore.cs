using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Characters.Abilities;

namespace Characters.Types
{
    public class Manticore : ACharacter
    {
        // ablitiy 1
        // ablitiy 2
        public AAbility AbilityFirst { get; protected set; }
        public AAbility AbilitySecond { get; protected set; }

        public Manticore(CharacterDto characterDto): base(characterDto)
        {
        }
    }
}
