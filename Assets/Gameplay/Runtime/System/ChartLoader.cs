using GuitarPoorGuy.Gameplay.Data;
using UnityEngine;

namespace GuitarPoorGuy.Gameplay.Systems
{
    public static class ChartLoader
    {
        public static RhythmChart FromJson(string chartJson)
        {
            if (string.IsNullOrWhiteSpace(chartJson))
            {
                Debug.LogError("Chart JSON is empty.");
                return null;
            }

            var chart = JsonUtility.FromJson<RhythmChart>(chartJson);
            if (chart == null || chart.notes == null)
            {
                Debug.LogError("Failed to parse chart JSON.");
                return null;
            }

            return chart;
        }
    }
}
