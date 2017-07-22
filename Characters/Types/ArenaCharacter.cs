namespace Characters.Types
{
    public class ArenaCharacter : ACharacter
    {
        private ArenaCharacter(CharacterDto characterDto) : base(characterDto)
        {
        }

        public override void Initialize(CharacterDto character)
        {
            InitializeBase(character);
        }
    }
}
