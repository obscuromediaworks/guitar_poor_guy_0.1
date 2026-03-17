using UnityEngine;
using UnityEngine.UI;

namespace GuitarPoorGuy.UI.Lanes
{
    public sealed class CountdownSequenceUI : MonoBehaviour
    {
        [SerializeField] private Text countdownText;
        [SerializeField] private string readyText = "Ready?";
        [SerializeField] private string goText = "GO!";
        [SerializeField] private float readyDurationSeconds = 1.0f;
        [SerializeField] private float pauseDurationSeconds = 0.5f;
        [SerializeField] private float goDurationSeconds = 0.8f;

        private float _startedAt;
        private bool _initialized;

        public bool IsFinished { get; private set; }
        public float TotalDurationSeconds => Mathf.Max(0f, readyDurationSeconds) + Mathf.Max(0f, pauseDurationSeconds) + Mathf.Max(0f, goDurationSeconds);

        private void OnEnable()
        {
            AutoResolveText();
            _startedAt = Time.unscaledTime;
            IsFinished = false;
            _initialized = true;
            Refresh();
        }

        private void Update()
        {
            if (!_initialized || IsFinished)
            {
                return;
            }

            Refresh();
        }

        private void Refresh()
        {
            var elapsed = Time.unscaledTime - _startedAt;
            var readyEnd = Mathf.Max(0f, readyDurationSeconds);
            var pauseEnd = readyEnd + Mathf.Max(0f, pauseDurationSeconds);
            var goEnd = pauseEnd + Mathf.Max(0f, goDurationSeconds);

            if (elapsed < readyEnd)
            {
                SetText(readyText, true);
                return;
            }

            if (elapsed < pauseEnd)
            {
                SetText(string.Empty, false);
                return;
            }

            if (elapsed < goEnd)
            {
                SetText(goText, true);
                return;
            }

            SetText(string.Empty, false);
            IsFinished = true;
        }

        private void SetText(string value, bool enabled)
        {
            if (countdownText == null)
            {
                return;
            }

            countdownText.enabled = enabled;
            if (countdownText.color.a < 0.95f)
            {
                var c = countdownText.color;
                c.a = 1f;
                countdownText.color = c;
            }

            countdownText.text = value;
        }

        private void AutoResolveText()
        {
            if (countdownText != null)
            {
                return;
            }

            var allTexts = FindObjectsByType<Text>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            for (var i = 0; i < allTexts.Length; i++)
            {
                if (allTexts[i].name.Contains("Countdown"))
                {
                    countdownText = allTexts[i];
                    return;
                }
            }

            if (allTexts.Length > 0)
            {
                countdownText = allTexts[0];
            }
        }
    }
}
