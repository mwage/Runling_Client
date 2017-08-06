using Characters.Abilities;
using Players;


namespace Characters.Types
{
    public class Manticore : ACharacter
    {
        public override void Initialize(CharacterDto character, PlayerManager playerManager)
        {
            InitializeBase(character);

            // something about abilities or in awake
            Ability1 = new Boost(this, playerManager);
            Ability2 = new Shield(this, playerManager);
        }
    }
}
