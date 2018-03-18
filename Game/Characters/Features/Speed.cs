namespace Game.Scripts.Characters.Features
{
    public class Speed
    {
        private readonly CharacterStats _stats;

        public float Current => BaseSpeed + SpeedPointRatio * _stats.SpeedPoints + _bonusSpeed;
        protected float BaseSpeed { get; private set; }
        protected float SpeedPointRatio { get; }
        private float _bonusSpeed;

        public Speed(CharacterStats stats, CharacterSettings settings)
        {
            _stats = stats;
            BaseSpeed = settings.BaseSpeed;
            SpeedPointRatio = settings.SpeedIncreasePerLevel;
        }
        
        public void ActivateBoost(float boostSpeed)
        {
            _bonusSpeed = boostSpeed;
        }

        public void DeactivateBoost(float boostSpeed)
        {
            _bonusSpeed = 0;
        }

        public void SetBaseSpeed (float newSpeed)
        {
            BaseSpeed = newSpeed;
        }
    }
}