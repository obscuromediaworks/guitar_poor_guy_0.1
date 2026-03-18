using System;
using GuitarPoorGuy.Audio;
using GuitarPoorGuy.Core.Time;
using GuitarPoorGuy.Gameplay.Data;
using GuitarPoorGuy.Gameplay.Input;
using GuitarPoorGuy.Gameplay.Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GuitarPoorGuy.Gameplay.Session
{
    public sealed class SongSessionController : MonoBehaviour
    {
        private const int LaneCount = 5;

        [Header("Data")]
        [SerializeField] private SongDefinition songDefinition;
        [SerializeField] private ChartSource chartSource;

        [Header("Refs")]
        [SerializeField] private SongTimeSource timeSource;
        [SerializeField] private MonoBehaviour audioServiceBehaviour;
        [SerializeField] private MonoBehaviour laneInputSourceBehaviour;
        [SerializeField] private UI.Lanes.CountdownSequenceUI countdownSequenceUI;

        [Header("Session")]
        [SerializeField] private bool enableQuickRestart = true;
        [SerializeField] private Key restartKey = Key.R;
        [SerializeField] private float gameplayTimingDelayMs = 0f;

        [Header("Judge Windows")]
        [SerializeField] private double perfectWindowMs = 40;
        [SerializeField] private double goodWindowMs = 90;

        private RhythmChart _chart;
        private HitJudge _judge;
        private ScoreSystem _score;
        private IAudioService _audioService;
        private ILaneInputSource _laneInputSource;
        private int[] _laneCursor;
        private string _activeSongId;

        public event Action<int, HitQuality> LaneNoteConsumed;
        public event Action SessionRestarted;

        public int CurrentScore => _score != null ? _score.Score : 0;
        public int CurrentCombo => _score != null ? _score.Combo : 0;
        public int CurrentMultiplier => _score != null ? _score.Multiplier : 1;
        public HitQuality LastHitQuality { get; private set; } = HitQuality.Miss;
        public double CurrentSongTimeMs => timeSource != null ? timeSource.SongTimeMs : 0;
        public RhythmChart ActiveChart => _chart;
        public bool IsSessionReady => _chart != null && timeSource != null;
        public int LaneCountRuntime => LaneCount;

        private void Awake()
        {
            ResolveReferences();
        }

        private void Start()
        {
            _judge = new HitJudge(perfectWindowMs, goodWindowMs);
            _score = new ScoreSystem();
            var resolvedChartSource = songDefinition != null && songDefinition.chartSource != null
                ? songDefinition.chartSource
                : chartSource;

            _chart = ChartLoader.FromJson(resolvedChartSource != null ? resolvedChartSource.chartJson : string.Empty);

            if (_chart == null)
            {
                enabled = false;
                return;
            }

            _activeSongId = ResolveSongId();
            ResolveTimingDelay();

            if (_laneInputSource == null)
            {
                Debug.LogError("Lane input source is missing. Assign one in inspector or add KeyboardLaneInputSource/InputSystemLaneInputSource in scene.");
                enabled = false;
                return;
            }

            if (timeSource == null)
            {
                Debug.LogError("SongTimeSource is missing. Add one to scene and assign it in SongSessionController.");
                enabled = false;
                return;
            }

            _laneCursor = new int[LaneCount];
            timeSource.Reset(_chart.offsetMs);
            _audioService?.PlaySong(_activeSongId);
        }

        private void Update()
        {
            HandleQuickRestart();

            for (var lane = 0; lane < LaneCount; lane++)
            {
                HandleLane(lane);
            }
        }

        private void ResolveReferences()
        {
            _audioService = audioServiceBehaviour as IAudioService;
            _laneInputSource = laneInputSourceBehaviour as ILaneInputSource;

            if (timeSource == null)
            {
                timeSource = FindFirstObjectByType<SongTimeSource>();
            }

            if (_audioService == null)
            {
                _audioService = GetComponent<IAudioService>() ?? FindAudioServiceInScene();
                if (_audioService is MonoBehaviour audioBehaviour)
                {
                    audioServiceBehaviour = audioBehaviour;
                }
            }

            if (_laneInputSource == null)
            {
                _laneInputSource = GetComponent<ILaneInputSource>() ?? FindLaneInputSourceInScene();
                if (_laneInputSource is MonoBehaviour inputBehaviour)
                {
                    laneInputSourceBehaviour = inputBehaviour;
                }
            }

            if (_laneInputSource == null)
            {
                var fallback = gameObject.AddComponent<KeyboardLaneInputSource>();
                _laneInputSource = fallback;
                laneInputSourceBehaviour = fallback;
                Debug.LogWarning("No lane input source assigned/found. Added KeyboardLaneInputSource fallback to SongSessionController GameObject.");
            }
        }

        private static IAudioService FindAudioServiceInScene()
        {
            var behaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            for (var i = 0; i < behaviours.Length; i++)
            {
                if (behaviours[i] is IAudioService service)
                {
                    return service;
                }
            }

            return null;
        }

        private static ILaneInputSource FindLaneInputSourceInScene()
        {
            var behaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            for (var i = 0; i < behaviours.Length; i++)
            {
                if (behaviours[i] is ILaneInputSource source)
                {
                    return source;
                }
            }

            return null;
        }

        private string ResolveSongId()
        {
            if (songDefinition != null && !string.IsNullOrWhiteSpace(songDefinition.songId))
            {
                return songDefinition.songId;
            }

            if (_chart != null && !string.IsNullOrWhiteSpace(_chart.songId))
            {
                return _chart.songId;
            }

            return "song_001";
        }


        private void ResolveTimingDelay()
        {
            if (countdownSequenceUI == null)
            {
                countdownSequenceUI = FindFirstObjectByType<UI.Lanes.CountdownSequenceUI>();
            }

            if (countdownSequenceUI != null && gameplayTimingDelayMs <= 0f)
            {
                gameplayTimingDelayMs = countdownSequenceUI.TotalDurationSeconds * 1000f;
            }
        }

        private void HandleQuickRestart()
        {
            if (!enableQuickRestart)
            {
                return;
            }

            var keyboard = Keyboard.current;
            if (keyboard == null || !keyboard[restartKey].wasPressedThisFrame)
            {
                return;
            }

            RestartSession();
        }

        [ContextMenu("Restart Session")]
        public void RestartSession()
        {
            if (_chart == null || timeSource == null)
            {
                return;
            }

            _audioService?.StopSong(_activeSongId);
            _score = new ScoreSystem();
            LastHitQuality = HitQuality.Miss;
            _laneCursor = new int[LaneCount];
            timeSource.Reset(_chart.offsetMs);
            _audioService?.PlaySong(_activeSongId);
            _audioService?.SetGuitarTrackLevel(0f);
            SessionRestarted?.Invoke();
        }

        private void HandleLane(int lane)
        {
            if (!_laneInputSource.WasLanePressedThisFrame(lane))
            {
                return;
            }

            _audioService?.PlayLanePress(lane);

            var note = FindNextNoteForLane(lane);
            if (note == null)
            {
                Register(HitQuality.Miss);
                return;
            }

            var expectedTimeMs = note.timeMs + gameplayTimingDelayMs;
            var quality = _judge.Evaluate(expectedTimeMs, timeSource.SongTimeMs);
            Register(quality);
            _laneCursor[lane]++;
            LaneNoteConsumed?.Invoke(lane, quality);
        }

        private ChartNote FindNextNoteForLane(int lane)
        {
            var index = _laneCursor[lane];
            for (var i = index; i < _chart.notes.Length; i++)
            {
                if (_chart.notes[i].lane == lane)
                {
                    _laneCursor[lane] = i;
                    return _chart.notes[i];
                }
            }

            return null;
        }

        private void Register(HitQuality quality)
        {
            LastHitQuality = quality;
            _score.Register(quality);
            _audioService?.PlayHit(quality);
            _audioService?.SetComboIntensity(_score.Combo / 100f);

            if (quality == HitQuality.Miss)
            {
                _audioService?.SetGuitarTrackLevel(0.1f);
                return;
            }

            _audioService?.PlayStrum(quality);
            _audioService?.SetGuitarTrackLevel(quality == HitQuality.Perfect ? 1f : 0.7f);
        }

        private void OnDisable()
        {
            if (_chart != null)
            {
                _audioService?.StopSong(_activeSongId);
            }
        }
    }
}
