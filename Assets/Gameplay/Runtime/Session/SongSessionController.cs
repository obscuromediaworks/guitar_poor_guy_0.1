using GuitarPoorGuy.Audio;
using GuitarPoorGuy.Core.Time;
using GuitarPoorGuy.Gameplay.Data;
using GuitarPoorGuy.Gameplay.Systems;
using UnityEngine;

namespace GuitarPoorGuy.Gameplay.Session
{
    public sealed class SongSessionController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private ChartSource chartSource;

        [Header("Refs")]
        [SerializeField] private SongTimeSource timeSource;
        [SerializeField] private MonoBehaviour audioServiceBehaviour;

        [Header("Judge Windows")]
        [SerializeField] private double perfectWindowMs = 40;
        [SerializeField] private double goodWindowMs = 90;

        private RhythmChart _chart;
        private HitJudge _judge;
        private ScoreSystem _score;
        private IAudioService _audioService;
        private int[] _laneCursor;

        private void Start()
        {
            _audioService = audioServiceBehaviour as IAudioService;
            _judge = new HitJudge(perfectWindowMs, goodWindowMs);
            _score = new ScoreSystem();
            _chart = ChartLoader.FromJson(chartSource != null ? chartSource.chartJson : string.Empty);

            if (_chart == null)
            {
                enabled = false;
                return;
            }

            _laneCursor = new int[5];
            timeSource.Reset(_chart.offsetMs);
            _audioService?.PlaySong(_chart.songId);
        }

        private void Update()
        {
            HandleLane(0, KeyCode.A);
            HandleLane(1, KeyCode.S);
            HandleLane(2, KeyCode.D);
            HandleLane(3, KeyCode.F);
            HandleLane(4, KeyCode.G);
        }

        private void HandleLane(int lane, KeyCode key)
        {
            if (!Input.GetKeyDown(key))
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
