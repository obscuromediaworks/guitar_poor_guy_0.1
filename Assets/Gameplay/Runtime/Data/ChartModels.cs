using System;
using UnityEngine;

namespace GuitarPoorGuy.Gameplay.Data
{
    [Serializable]
    public sealed class RhythmChart
    {
        public int formatVersion = 1;
        public string songId = "song_001";
        public float bpm = 120f;
        public float offsetMs = 0f;
        public ChartNote[] notes = Array.Empty<ChartNote>();
    }

    [Serializable]
    public sealed class ChartNote
    {
        public float timeMs;
        public int lane;
        public string type = "tap";
        public float durationMs;
        public int[] chordLanes = Array.Empty<int>();
    }

    [CreateAssetMenu(menuName = "GuitarPoorGuy/Chart Source", fileName = "ChartSource")]
    public sealed class ChartSource : ScriptableObject
    {
        [TextArea(10, 40)]
        public string chartJson;
    }
}
