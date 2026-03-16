using GuitarPoorGuy.Gameplay.Session;
using UnityEngine;
using UnityEngine.UI;

namespace GuitarPoorGuy.UI
{
    public sealed class SongHudController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SongSessionController sessionController;

        [Header("Text Widgets")]
        [SerializeField] private Text scoreText;
        [SerializeField] private Text comboText;
        [SerializeField] private Text multiplierText;
        [SerializeField] private Text hitQualityText;
        [SerializeField] private Text songTimeText;

        [Header("Refresh")]
        [SerializeField] private float refreshRateHz = 20f;

        private float _nextRefreshTime;

        private void Awake()
        {
            if (sessionController == null)
            {
                sessionController = FindFirstObjectByType<SongSessionController>();
            }

            RefreshNow();
        }

        private void Update()
        {
            if (UnityEngine.Time.unscaledTime < _nextRefreshTime)
            {
                return;
            }

            _nextRefreshTime = UnityEngine.Time.unscaledTime + (1f / Mathf.Max(1f, refreshRateHz));
            RefreshNow();
        }

        private void RefreshNow()
        {
            if (sessionController == null)
            {
                return;
            }

            SetText(scoreText, $"Score: {sessionController.CurrentScore}");
            SetText(comboText, $"Combo: {sessionController.CurrentCombo}");
            SetText(multiplierText, $"Multiplier: x{sessionController.CurrentMultiplier}");
            SetText(hitQualityText, $"Last Hit: {sessionController.LastHitQuality}");
            SetText(songTimeText, $"Song: {(sessionController.CurrentSongTimeMs / 1000.0):0.00}s");
        }

        private static void SetText(Text textWidget, string value)
        {
            if (textWidget != null)
            {
                textWidget.text = value;
            }
        }
    }
}
