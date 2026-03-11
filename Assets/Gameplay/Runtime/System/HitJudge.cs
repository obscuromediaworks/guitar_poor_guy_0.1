using System;

namespace GuitarPoorGuy.Gameplay.Systems
{
    public enum HitQuality
    {
        Perfect,
        Good,
        Miss
    }

    public sealed class HitJudge
    {
        private readonly double _perfectWindowMs;
        private readonly double _goodWindowMs;

        public HitJudge(double perfectWindowMs, double goodWindowMs)
        {
            _perfectWindowMs = perfectWindowMs;
            _goodWindowMs = goodWindowMs;
        }

        public HitQuality Evaluate(double expectedTimeMs, double inputTimeMs)
        {
            var delta = Math.Abs(inputTimeMs - expectedTimeMs);
            if (delta <= _perfectWindowMs)
            {
                return HitQuality.Perfect;
            }

            if (delta <= _goodWindowMs)
            {
                return HitQuality.Good;
            }

            return HitQuality.Miss;
        }
    }
}
