using Characters.Abilities;


namespace Characters.Types
{
    public class Manticore : ACharacter
    {
        private Manticore(CharacterDto characterDto): base(characterDto)
        {
        }

        public override void Initialize(CharacterDto character)
        {
            InitializeBase(character);
            // something about abilities or in awake
            Ability1 = new Boost(this);
            Ability2 = new Shield(this);
        }


    }
}
