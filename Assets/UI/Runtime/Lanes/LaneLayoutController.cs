using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuitarPoorGuy.UI.Lanes
{
    public sealed class LaneLayoutController : MonoBehaviour
    {
        [SerializeField] private RectTransform lanesRoot;
        [SerializeField] private LaneVisualTheme theme;
        [SerializeField] private bool rebuildOnAwake = true;

        private readonly List<RectTransform> _laneContainers = new List<RectTransform>();
        private RectTransform _hitLineRect;

        public IReadOnlyList<RectTransform> LaneContainers => _laneContainers;
        public LaneVisualTheme Theme => theme;

        private void Awake()
        {
            if (rebuildOnAwake)
            {
                Build();
            }
        }

        [ContextMenu("Build Lanes")]
        public void Build()
        {
            if (lanesRoot == null || theme == null)
            {
                return;
            }

            ClearChildren();
            _laneContainers.Clear();

            var totalWidth = (theme.laneWidth * theme.laneCount) + (theme.laneSpacing * (theme.laneCount - 1));
            var startX = -totalWidth * 0.5f + (theme.laneWidth * 0.5f);

            for (var lane = 0; lane < theme.laneCount; lane++)
            {
                var laneObject = new GameObject($"Lane_{lane}", typeof(RectTransform), typeof(Image));
                laneObject.transform.SetParent(lanesRoot, false);

                var laneRect = (RectTransform)laneObject.transform;
                laneRect.anchorMin = new Vector2(0.5f, 0.5f);
                laneRect.anchorMax = new Vector2(0.5f, 0.5f);
                laneRect.sizeDelta = new Vector2(theme.laneWidth, theme.laneHeight);
                laneRect.anchoredPosition = new Vector2(startX + lane * (theme.laneWidth + theme.laneSpacing), 0f);

                var laneImage = laneObject.GetComponent<Image>();
                laneImage.color = theme.laneBaseColor;

                var accentObject = new GameObject("Accent", typeof(RectTransform), typeof(Image));
                accentObject.transform.SetParent(laneRect, false);
                var accentRect = (RectTransform)accentObject.transform;
                accentRect.anchorMin = new Vector2(0f, 1f);
                accentRect.anchorMax = new Vector2(1f, 1f);
                accentRect.pivot = new Vector2(0.5f, 1f);
                accentRect.sizeDelta = new Vector2(0f, 8f);
                accentRect.anchoredPosition = Vector2.zero;
                var accentImage = accentObject.GetComponent<Image>();
                accentImage.color = theme.GetLaneAccentColor(lane);

                _laneContainers.Add(laneRect);
            }

            var hitLineObject = new GameObject("HitLine", typeof(RectTransform), typeof(Image));
            hitLineObject.transform.SetParent(lanesRoot, false);
            _hitLineRect = (RectTransform)hitLineObject.transform;
            _hitLineRect.anchorMin = new Vector2(0.5f, 0f);
            _hitLineRect.anchorMax = new Vector2(0.5f, 0f);
            _hitLineRect.sizeDelta = new Vector2(totalWidth + 20f, 6f);
            _hitLineRect.anchoredPosition = new Vector2(0f, -theme.laneHeight * 0.45f);

            var hitLineImage = hitLineObject.GetComponent<Image>();
            hitLineImage.color = theme.hitLineColor;
        }

        private void ClearChildren()
        {
            for (var i = lanesRoot.childCount - 1; i >= 0; i--)
            {
                var child = lanesRoot.GetChild(i);
                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}
