using GuitarPoorGuy.Gameplay.Systems;

namespace GuitarPoorGuy.Audio
{
    public interface IAudioService
    {
        void PlaySong(string songId);
        void StopSong(string songId);
        void PlayHit(HitQuality quality);
        void PlayLanePress(int lane);
        void PlayStrum(HitQuality quality);
        void SetComboIntensity(float normalized);
        void SetGuitarTrackLevel(float normalized);
    }
}
