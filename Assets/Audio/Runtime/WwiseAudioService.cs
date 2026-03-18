using GuitarPoorGuy.Gameplay.Systems;
using UnityEngine;

namespace GuitarPoorGuy.Audio
{
    public sealed class WwiseAudioService : MonoBehaviour, IAudioService
    {
        [Header("Events")]
        [SerializeField] private string lanePressEvent = "Play_SFX_LanePress";
        [SerializeField] private string strumEvent = "Play_SFX_Strum";
        [SerializeField] private string hitEvent = "Play_SFX_Hit";
        [SerializeField] private string missEvent = "Play_SFX_Miss";

        [Header("RTPC")]
        [SerializeField] private string comboRtpc = "RTPC_Player_ComboIntensity";
        [SerializeField] private string guitarLayerRtpc = "RTPC_Music_GuitarLayer";

        [Header("Switch")]
        [SerializeField] private string hitQualitySwitchGroup = "HitQuality";

        public void PlaySong(string songId)
        {
            PostEvent($"Play_Music_{songId}");
        }

        public void StopSong(string songId)
        {
            PostEvent($"Stop_Music_{songId}");
        }

        public void PlayHit(HitQuality quality)
        {
            SetSwitch(hitQualitySwitchGroup, quality.ToString());
            PostEvent(quality == HitQuality.Miss ? missEvent : hitEvent);
        }

        public void PlayLanePress(int lane)
        {
            PostEvent(lanePressEvent);
        }

        public void PlayStrum(HitQuality quality)
        {
            if (quality == HitQuality.Miss)
            {
                return;
            }

            PostEvent(strumEvent);
        }

        public void SetComboIntensity(float normalized)
        {
            SetRtpc(comboRtpc, Mathf.Clamp01(normalized) * 100f);
        }

        public void SetGuitarTrackLevel(float normalized)
        {
            SetRtpc(guitarLayerRtpc, Mathf.Clamp01(normalized) * 100f);
        }

        private void PostEvent(string eventName)
        {
#if AK_WWISE_ADDRESSABLES || AK_WWISE
            AkSoundEngine.PostEvent(eventName, gameObject);
#else
            Debug.Log($"[WwiseAudioService] PostEvent {eventName}");
#endif
        }

        private void SetRtpc(string rtpcName, float value)
        {
#if AK_WWISE_ADDRESSABLES || AK_WWISE
            AkSoundEngine.SetRTPCValue(rtpcName, value, gameObject);
#else
            Debug.Log($"[WwiseAudioService] SetRTPC {rtpcName}={value:0.00}");
#endif
        }

        private void SetSwitch(string switchGroup, string switchValue)
        {
#if AK_WWISE_ADDRESSABLES || AK_WWISE
            AkSoundEngine.SetSwitch(switchGroup, switchValue, gameObject);
#else
            Debug.Log($"[WwiseAudioService] SetSwitch {switchGroup}/{switchValue}");
#endif
        }
    }
}
