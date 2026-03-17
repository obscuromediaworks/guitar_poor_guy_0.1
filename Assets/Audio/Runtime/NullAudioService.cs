using GuitarPoorGuy.Gameplay.Systems;
using UnityEngine;

namespace GuitarPoorGuy.Audio
{
    public sealed class NullAudioService : MonoBehaviour, IAudioService
    {
        public void PlaySong(string songId)
        {
            Debug.Log($"[NullAudioService] PlaySong {songId}");
        }

        public void StopSong(string songId)
        {
            Debug.Log($"[NullAudioService] StopSong {songId}");
        }

        public void PlayHit(HitQuality quality)
        {
            Debug.Log($"[NullAudioService] Hit {quality}");
        }

        public void SetComboIntensity(float normalized)
        {
            Debug.Log($"[NullAudioService] Combo RTPC {normalized:0.00}");
        }
    }
}
