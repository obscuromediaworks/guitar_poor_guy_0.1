using System;
using System.Collections.Generic;
using GuitarPoorGuy.Gameplay.Data;
using UnityEditor;
using UnityEngine;

namespace GuitarPoorGuy.Gameplay.Editor
{
    public sealed class ChartAuthoringWindow : EditorWindow
    {
        private ChartSource _chartSource;
        private RhythmChart _chart;
        private Vector2 _scroll;

        private int _newLane;
        private float _newTimeMs;
        private string _newType = "tap";
        private float _newDurationMs = 500f;

        [MenuItem("GuitarPoorGuy/Chart Authoring Window")]
        public static void Open()
        {
            GetWindow<ChartAuthoringWindow>("Chart Authoring");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Chart Authoring Tool", EditorStyles.boldLabel);

            var selected = (ChartSource)EditorGUILayout.ObjectField("Chart Source", _chartSource, typeof(ChartSource), false);
            if (selected != _chartSource)
            {
                _chartSource = selected;
                LoadFromSource();
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Load"))
                {
                    LoadFromSource();
                }

                if (GUILayout.Button("Save"))
                {
                    SaveToSource();
                }

                if (GUILayout.Button("Sort by Time"))
                {
                    SortByTime();
                }
            }

            if (_chart == null)
            {
                EditorGUILayout.HelpBox("Assign a ChartSource and click Load.", MessageType.Info);
                return;
            }

            DrawHeaderFields();
            DrawAddNoteSection();
            DrawNotesList();
        }

        private void DrawHeaderFields()
        {
            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Song", EditorStyles.boldLabel);
            _chart.songId = EditorGUILayout.TextField("Song Id", _chart.songId);
            _chart.bpm = EditorGUILayout.FloatField("BPM", _chart.bpm);
            _chart.offsetMs = EditorGUILayout.FloatField("Offset Ms", _chart.offsetMs);
        }

        private void DrawAddNoteSection()
        {
            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Add Note", EditorStyles.boldLabel);

            _newTimeMs = EditorGUILayout.FloatField("Time Ms", _newTimeMs);
            _newLane = EditorGUILayout.IntSlider("Lane", _newLane, 0, 4);
            _newType = EditorGUILayout.Popup("Type", TypeIndex(_newType), new[] { "tap", "sustain", "chord" }) switch
            {
                0 => "tap",
                1 => "sustain",
                _ => "chord"
            };

            if (_newType == "sustain")
            {
                _newDurationMs = EditorGUILayout.FloatField("Duration Ms", _newDurationMs);
            }

            if (GUILayout.Button("Add Note"))
            {
                AddNote();
            }
        }

        private void DrawNotesList()
        {
            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Notes", EditorStyles.boldLabel);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            var notes = _chart.notes ?? Array.Empty<ChartNote>();
            for (var i = 0; i < notes.Length; i++)
            {
                var note = notes[i];

                using (new EditorGUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField($"#{i}");
                    note.timeMs = EditorGUILayout.FloatField("Time Ms", note.timeMs);
                    note.lane = EditorGUILayout.IntSlider("Lane", note.lane, 0, 4);
                    note.type = EditorGUILayout.Popup("Type", TypeIndex(note.type), new[] { "tap", "sustain", "chord" }) switch
                    {
                        0 => "tap",
                        1 => "sustain",
                        _ => "chord"
                    };

                    if (note.type == "sustain")
                    {
                        note.durationMs = EditorGUILayout.FloatField("Duration Ms", note.durationMs);
                    }

                    if (GUILayout.Button("Delete"))
                    {
                        RemoveAt(i);
                        break;
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void AddNote()
        {
            var list = new List<ChartNote>(_chart.notes ?? Array.Empty<ChartNote>());
            var note = new ChartNote
            {
                timeMs = Mathf.Max(0f, _newTimeMs),
                lane = Mathf.Clamp(_newLane, 0, 4),
                type = _newType,
                durationMs = Mathf.Max(0f, _newDurationMs)
            };

            if (note.type == "chord")
            {
                note.chordLanes = new[] { Mathf.Clamp(_newLane, 0, 4), Mathf.Clamp(_newLane + 1, 0, 4) };
            }

            list.Add(note);
            _chart.notes = list.ToArray();
            SortByTime();
        }

        private void RemoveAt(int index)
        {
            var list = new List<ChartNote>(_chart.notes ?? Array.Empty<ChartNote>());
            if (index < 0 || index >= list.Count)
            {
                return;
            }

            list.RemoveAt(index);
            _chart.notes = list.ToArray();
        }

        private void SortByTime()
        {
            var list = new List<ChartNote>(_chart.notes ?? Array.Empty<ChartNote>());
            list.Sort((a, b) => a.timeMs.CompareTo(b.timeMs));
            _chart.notes = list.ToArray();
        }

        private void LoadFromSource()
        {
            if (_chartSource == null)
            {
                _chart = null;
                return;
            }

            if (string.IsNullOrWhiteSpace(_chartSource.chartJson))
            {
                _chart = new RhythmChart();
                return;
            }

            _chart = JsonUtility.FromJson<RhythmChart>(_chartSource.chartJson);
            if (_chart == null)
            {
                _chart = new RhythmChart();
            }
        }

        private void SaveToSource()
        {
            if (_chartSource == null || _chart == null)
            {
                return;
            }

            _chartSource.chartJson = JsonUtility.ToJson(_chart, true);
            EditorUtility.SetDirty(_chartSource);
            AssetDatabase.SaveAssets();
        }

        private static int TypeIndex(string noteType)
        {
            return noteType switch
            {
                "tap" => 0,
                "sustain" => 1,
                "chord" => 2,
                _ => 0
            };
        }
    }
}
