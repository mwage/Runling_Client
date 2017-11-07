using Characters.Abilities;
using Characters.Repositories;
using Players;

namespace Characters.Types
{
    public class Cat : ACharacter
    {
        public override void Initialize(PlayerManager playerManager, CharacterDto character = null)
        {
            // Arena character
            if (character == null)
            {
                InitializeSpeed();
            }
            // RLR Character
            else
            {
                Character = character;
                InitializeFromDto();
                Ability1 = new Boost(this, playerManager);
                Ability2 = new Shield(this, playerManager);
            }
        }
    }
}
