using GuitarPoorGuy.Audio;
using GuitarPoorGuy.Core.Time;
using GuitarPoorGuy.Gameplay.Data;
using GuitarPoorGuy.Gameplay.Input;
using GuitarPoorGuy.Gameplay.Systems;
using UnityEngine;

namespace GuitarPoorGuy.Gameplay.Session
{
    public sealed class SongSessionController : MonoBehaviour
    {
        private const int LaneCount = 5;

        [Header("Data")]
        [SerializeField] private ChartSource chartSource;

        [Header("Refs")]
        [SerializeField] private SongTimeSource timeSource;
        [SerializeField] private MonoBehaviour audioServiceBehaviour;
        [SerializeField] private MonoBehaviour laneInputSourceBehaviour;

        [Header("Judge Windows")]
        [SerializeField] private double perfectWindowMs = 40;
        [SerializeField] private double goodWindowMs = 90;

        private RhythmChart _chart;
        private HitJudge _judge;
        private ScoreSystem _score;
        private IAudioService _audioService;
        private ILaneInputSource _laneInputSource;
        private int[] _laneCursor;

        private void Awake()
        {
            ResolveReferences();
        }

        private void Start()
        {
            _judge = new HitJudge(perfectWindowMs, goodWindowMs);
            _score = new ScoreSystem();
            _chart = ChartLoader.FromJson(chartSource != null ? chartSource.chartJson : string.Empty);

            if (_chart == null)
            {
                enabled = false;
                return;
            }

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
            _audioService?.PlaySong(_chart.songId);
        }

        private void Update()
        {
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

        private void HandleLane(int lane)
        {
            if (!_laneInputSource.WasLanePressedThisFrame(lane))
            {
                return;
            }

            var note = FindNextNoteForLane(lane);
            if (note == null)
            {
                Register(HitQuality.Miss);
                return;
            }

            var quality = _judge.Evaluate(note.timeMs, timeSource.SongTimeMs);
            Register(quality);
            _laneCursor[lane]++;
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
            _score.Register(quality);
            _audioService?.PlayHit(quality);
            _audioService?.SetComboIntensity(_score.Combo / 100f);
        }

        private void OnDisable()
        {
            if (_chart != null)
            {
                _audioService?.StopSong(_chart.songId);
            }
        }
    }
}
