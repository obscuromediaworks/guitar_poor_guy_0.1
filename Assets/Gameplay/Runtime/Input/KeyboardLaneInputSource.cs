using UnityEngine;
using UnityEngine.InputSystem;

namespace GuitarPoorGuy.Gameplay.Input
{
    public sealed class KeyboardLaneInputSource : MonoBehaviour, ILaneInputSource
    {
        [SerializeField] private Key lane0 = Key.A;
        [SerializeField] private Key lane1 = Key.S;
        [SerializeField] private Key lane2 = Key.D;
        [SerializeField] private Key lane3 = Key.F;
        [SerializeField] private Key lane4 = Key.G;

        public bool WasLanePressedThisFrame(int laneIndex)
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return false;
            }

            return laneIndex switch
            {
                0 => keyboard[lane0].wasPressedThisFrame,
                1 => keyboard[lane1].wasPressedThisFrame,
                2 => keyboard[lane2].wasPressedThisFrame,
                3 => keyboard[lane3].wasPressedThisFrame,
                4 => keyboard[lane4].wasPressedThisFrame,
                _ => false
            };
        }
    }
}
