using System.Collections.Generic;
using GuitarPoorGuy.Gameplay.Session;
using UnityEngine;
using UnityEngine.UI;

namespace GuitarPoorGuy.UI.Lanes
{
    public sealed class LaneNoteScroller : MonoBehaviour
    {
        [SerializeField] private SongSessionController sessionController;
        [SerializeField] private LaneLayoutController laneLayoutController;
        [SerializeField] private CountdownSequenceUI countdownSequenceUI;
        [SerializeField] private float retryBuildIntervalSeconds = 0.25f;
        [SerializeField] private float noteFallStartDelayMs = 0f;

        private readonly List<RectTransform> _activeNoteRects = new List<RectTransform>();
        private readonly List<float> _activeNoteTimes = new List<float>();

        private bool _built;
        private float _nextBuildAttemptTime;

        private void Start()
        {
            if (sessionController == null)
            {
                sessionController = FindFirstObjectByType<SongSessionController>();
            }

            if (laneLayoutController == null)
            {
                laneLayoutController = FindFirstObjectByType<LaneLayoutController>();
            }

            if (countdownSequenceUI == null)
            {
                countdownSequenceUI = FindFirstObjectByType<CountdownSequenceUI>();
            }

            if (countdownSequenceUI != null && noteFallStartDelayMs <= 0f)
            {
                noteFallStartDelayMs = countdownSequenceUI.TotalDurationSeconds * 1000f;
            }

            TryBuildNotes();
        }

        private void Update()
        {
            if (countdownSequenceUI != null && !countdownSequenceUI.IsFinished)
            {
                return;
            }

            if (!_built)
            {
                TryBuildNotesPeriodically();
                return;
            }

            if (sessionController == null || laneLayoutController == null || laneLayoutController.Theme == null)
            {
                return;
            }

            var theme = laneLayoutController.Theme;
            var nowMs = (float)sessionController.CurrentSongTimeMs - Mathf.Max(0f, noteFallStartDelayMs);

            for (var i = 0; i < _activeNoteRects.Count; i++)
            {
                var noteRect = _activeNoteRects[i];
                var deltaMs = _activeNoteTimes[i] - nowMs;
                var normalized = deltaMs / theme.noteLeadTimeMs;
                var y = Mathf.Lerp(-theme.laneHeight * 0.45f, theme.noteTravelHeight * 0.5f, normalized);
                noteRect.anchoredPosition = new Vector2(0f, y);

                var shouldShow = deltaMs > -200f && deltaMs < theme.noteLeadTimeMs;
                if (noteRect.gameObject.activeSelf != shouldShow)
                {
                    noteRect.gameObject.SetActive(shouldShow);
                }
            }
        }

        [ContextMenu("Rebuild Notes")]
        public void BuildNotes()
        {
            TryBuildNotes(force: true);
        }

        private void TryBuildNotesPeriodically()
        {
            if (Time.unscaledTime < _nextBuildAttemptTime)
            {
                return;
            }

            _nextBuildAttemptTime = Time.unscaledTime + Mathf.Max(0.05f, retryBuildIntervalSeconds);
            TryBuildNotes();
        }

        private bool TryBuildNotes(bool force = false)
        {
            if (sessionController == null)
            {
                sessionController = FindFirstObjectByType<SongSessionController>();
            }

            if (laneLayoutController == null)
            {
                laneLayoutController = FindFirstObjectByType<LaneLayoutController>();
            }

            if (sessionController == null || laneLayoutController == null || laneLayoutController.Theme == null)
            {
                return false;
            }

            if (!force && !sessionController.IsSessionReady)
            {
                return false;
            }

            if (!force && countdownSequenceUI != null && !countdownSequenceUI.IsFinished)
            {
                return false;
            }

            var chart = sessionController.ActiveChart;
            if (chart == null || chart.notes == null || chart.notes.Length == 0)
            {
                return false;
            }

            ClearNotes();

            var laneContainers = laneLayoutController.LaneContainers;
            if (laneContainers == null || laneContainers.Count == 0)
            {
                laneLayoutController.Build();
                laneContainers = laneLayoutController.LaneContainers;
            }

            var theme = laneLayoutController.Theme;

            for (var i = 0; i < chart.notes.Length; i++)
            {
                var note = chart.notes[i];
                if (note.lane < 0 || note.lane >= laneContainers.Count)
                {
                    continue;
                }

                var noteObject = new GameObject($"Note_{i}", typeof(RectTransform), typeof(Image), typeof(NoteView));
                noteObject.transform.SetParent(laneContainers[note.lane], false);

                var noteRect = (RectTransform)noteObject.transform;
                noteRect.anchorMin = new Vector2(0.5f, 0f);
                noteRect.anchorMax = new Vector2(0.5f, 0f);
                noteRect.sizeDelta = new Vector2(theme.laneWidth * 0.75f, theme.noteHeight);
                noteRect.anchoredPosition = new Vector2(0f, theme.noteTravelHeight * 0.5f);

                var image = noteObject.GetComponent<Image>();
                image.color = theme.GetLaneAccentColor(note.lane);

                var noteView = noteObject.GetComponent<NoteView>();
                noteView.Initialize(note.lane, note.timeMs, theme.GetLaneAccentColor(note.lane));

                _activeNoteRects.Add(noteRect);
                _activeNoteTimes.Add(note.timeMs);
            }

            _built = _activeNoteRects.Count > 0;
            return _built;
        }

        private void ClearNotes()
        {
            for (var i = 0; i < _activeNoteRects.Count; i++)
            {
                if (_activeNoteRects[i] == null)
                {
                    continue;
                }

                if (Application.isPlaying)
                {
                    Destroy(_activeNoteRects[i].gameObject);
                }
                else
                {
                    DestroyImmediate(_activeNoteRects[i].gameObject);
                }
            }

            _activeNoteRects.Clear();
            _activeNoteTimes.Clear();
            _built = false;
        }
    }
}
