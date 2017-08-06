using Characters.Abilities;
using Players;


namespace Characters.Types
{
    public class Unicorn : ACharacter
    {
        public override void Initialize(CharacterDto character, PlayerManager playerManager)
        {
            InitializeBase(character);
            Ability1 = new Freeze(this, playerManager);
            Ability2 = new GravityField(this, playerManager);
        }
    }
}
