namespace Characters.Types.Features
{
    public class Speed
    {
        public int Points { get; private set; }
        
        public float Current => BaseSpeed + SpeedPointRatio * Points + _bonusSpeed;
        protected float BaseSpeed { get; private set; } = 4;
        protected float SpeedPointRatio { get; } = 0.05f;
        private float _bonusSpeed;

        public Speed(float baseSpeed, float speedPointRatio, int points = 0)
        {
            BaseSpeed = baseSpeed;
            SpeedPointRatio = speedPointRatio;
            Points = points;
        }

        public Speed (int points = 0)
        {
            Points = points;
        }

        public void IncrementPoints()
        {
            Points++;
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