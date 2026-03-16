using System;
using UnityEngine;

namespace GuitarPoorGuy.UI.Lanes
{
    [CreateAssetMenu(menuName = "GuitarPoorGuy/UI/Lane Visual Theme", fileName = "LaneVisualTheme")]
    public sealed class LaneVisualTheme : ScriptableObject
    {
        [Header("Layout")]
        [Min(1)] public int laneCount = 5;
        [Min(10f)] public float laneWidth = 120f;
        [Min(0f)] public float laneSpacing = 12f;
        [Min(100f)] public float laneHeight = 700f;

        [Header("Colors")]
        public Color laneBaseColor = new Color(0.18f, 0.18f, 0.2f, 0.85f);
        public Color laneBorderColor = new Color(0.35f, 0.35f, 0.4f, 0.95f);
        public Color hitLineColor = new Color(0.95f, 0.95f, 0.35f, 1f);
        public Color[] laneAccentColors =
        {
            new Color(0.30f, 0.90f, 0.45f, 0.85f),
            new Color(0.95f, 0.30f, 0.30f, 0.85f),
            new Color(0.25f, 0.55f, 0.95f, 0.85f),
            new Color(1f, 0.72f, 0.20f, 0.85f),
            new Color(0.80f, 0.40f, 0.95f, 0.85f)
        };

        [Header("Note")]
        [Min(16f)] public float noteHeight = 30f;
        [Min(100f)] public float noteTravelHeight = 520f;

        [Header("Timing")]
        [Min(100f)] public float noteLeadTimeMs = 1800f;

        public Color GetLaneAccentColor(int lane)
        {
            if (laneAccentColors == null || laneAccentColors.Length == 0)
            {
                return laneBaseColor;
            }

            return laneAccentColors[Mathf.Abs(lane) % laneAccentColors.Length];
        }
    }
}
