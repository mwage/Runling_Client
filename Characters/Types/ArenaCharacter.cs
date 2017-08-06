using Players;

namespace Characters.Types
{
    public class ArenaCharacter : ACharacter
    {
        public override void Initialize(CharacterDto character, PlayerManager playerManager)
        {
            InitializeBase(character);
        }
    }
}
