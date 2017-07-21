using Characters.Abilities;


namespace Characters.Types
{
    public class Manticore : ACharacter
    {
        // ablitiy 1
        // ablitiy 2

        private Manticore(CharacterDto characterDto): base(characterDto)
        {
        }

        public override void Initialize(CharacterDto character)
        {
            InitializeBase(character);
            // something about abilities or in awake
            AbilityFirst = new Boost(this);
            AbilitySecond = new Shield(this);
        }


    }
}
