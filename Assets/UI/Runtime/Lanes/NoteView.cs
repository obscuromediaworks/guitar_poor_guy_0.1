using UnityEngine;
using UnityEngine.UI;

namespace GuitarPoorGuy.UI.Lanes
{
    public sealed class NoteView : MonoBehaviour
    {
        [SerializeField] private Image image;

        public int Lane { get; private set; }
        public float NoteTimeMs { get; private set; }

        public void Initialize(int lane, float noteTimeMs, Color color)
        {
            Lane = lane;
            NoteTimeMs = noteTimeMs;
            if (image != null)
            {
                image.color = color;
            }
        }
    }
}
