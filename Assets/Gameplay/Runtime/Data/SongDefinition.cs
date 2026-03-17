using UnityEngine;

namespace GuitarPoorGuy.Gameplay.Data
{
    [CreateAssetMenu(menuName = "GuitarPoorGuy/Song Definition", fileName = "SongDefinition")]
    public sealed class SongDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string songId = "song_001";

        [Header("Gameplay Data")]
        public ChartSource chartSource;

        [Header("Audio Routing")]
        [Tooltip("If empty, uses songId to build Play_Music_<songId> in audio service layer.")]
        public string playMusicEventOverride;

        [Tooltip("If empty, uses songId to build Stop_Music_<songId> in audio service layer.")]
        public string stopMusicEventOverride;
    }
}
