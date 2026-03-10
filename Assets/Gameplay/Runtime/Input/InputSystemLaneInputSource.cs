using UnityEngine;
using UnityEngine.InputSystem;

namespace GuitarPoorGuy.Gameplay.Input
{
    public sealed class InputSystemLaneInputSource : MonoBehaviour, ILaneInputSource
    {
        [SerializeField] private InputActionReference[] laneActions = new InputActionReference[5];

        private void OnEnable()
        {
            if (laneActions == null)
            {
                return;
            }

            for (var i = 0; i < laneActions.Length; i++)
            {
                laneActions[i]?.action?.Enable();
            }
        }

        private void OnDisable()
        {
            if (laneActions == null)
            {
                return;
            }

            for (var i = 0; i < laneActions.Length; i++)
            {
                laneActions[i]?.action?.Disable();
            }
        }

        public bool WasLanePressedThisFrame(int laneIndex)
        {
            if (laneActions == null || laneIndex < 0 || laneIndex >= laneActions.Length)
            {
                return false;
            }

            var action = laneActions[laneIndex]?.action;
            return action != null && action.WasPressedThisFrame();
        }
    }
}
