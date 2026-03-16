using UnityEngine;

namespace GuitarPoorGuy.Core.Time
{
    public sealed class SongTimeSource : MonoBehaviour, ITimeSource
    {
        [SerializeField] private double startTimeMs;
        [SerializeField] private bool autoTick = true;

        public double SongTimeMs { get; private set; }

        private void Awake()
        {
            Reset(startTimeMs);
        }

        private void Update()
        {
            if (autoTick)
            {
                Tick(UnityEngine.Time.deltaTime);
            }
        }

        public void Reset(double startAtMs = 0)
        {
            SongTimeMs = startAtMs;
        }

        public void Tick(double deltaTimeSeconds)
        {
            SongTimeMs += deltaTimeSeconds * 1000.0;
        }
    }
}
