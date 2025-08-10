// ------------------------------------------------------------------------------
// File: PlayerStateSwitcher.cs
// Description: Switches between 2D and 3D player representations.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    public class PlayerStateSwitcher : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject player2D;
        [SerializeField] private GameObject player3D;
        [SerializeField] private CameraController cameraController;

        private void Start()
        {
            ApplyMode(GameState.Instance.Mode);
            GameState.Instance.OnModeChanged += ApplyMode;
        }

        private void OnDestroy()
        {
            if (GameState.Instance != null)
                GameState.Instance.OnModeChanged -= ApplyMode;
        }

        public void ToggleMode()
        {
            var next = GameState.Instance.Mode == DimensionMode.Mode2D ? DimensionMode.Mode3D : DimensionMode.Mode2D;
            GameState.Instance.SetMode(next);
        }

        private void ApplyMode(DimensionMode mode)
        {
            bool to3D = mode == DimensionMode.Mode3D;
            if (player2D) player2D.SetActive(!to3D);
            if (player3D) player3D.SetActive(to3D);
            if (cameraController) cameraController.SwitchCamera(to3D);
        }
    }
}
