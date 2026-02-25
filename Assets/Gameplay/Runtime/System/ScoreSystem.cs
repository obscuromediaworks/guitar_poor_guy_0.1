namespace GuitarPoorGuy.Gameplay.Systems
{
    public sealed class ScoreSystem
    {
        public int Score { get; private set; }
        public int Combo { get; private set; }
        public int Multiplier { get; private set; } = 1;

        public void Register(HitQuality quality)
        {
            if (quality == HitQuality.Miss)
            {
                Combo = 0;
                Multiplier = 1;
                return;
            }

            Combo++;
            Multiplier = 1 + (Combo / 10);
            if (Multiplier > 4)
            {
                Multiplier = 4;
            }

            var basePoints = quality == HitQuality.Perfect ? 100 : 70;
            Score += basePoints * Multiplier;
        }
    }
}
