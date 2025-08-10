// ------------------------------------------------------------------------------
// File: HiddenPath2D.cs
// Description: Only active/visible in 2D mode.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    public class HiddenPath2D : MonoBehaviour
    {
        [SerializeField] private Collider colliderRef; // 3D Collider
        [SerializeField] private Collider2D collider2DRef; // 2D Collider
        [SerializeField] private Renderer[] renderers;

        private void Start()
        {
            if (GameState.Instance != null)
            {
                Apply(GameState.Instance.Mode);
                GameState.Instance.OnModeChanged += Apply;
            }
        }

        private void OnDestroy()
        {
            if (GameState.Instance != null)
                GameState.Instance.OnModeChanged -= Apply;
        }

        private void Apply(DimensionMode mode)
        {
            bool active = mode == DimensionMode.Mode2D;
            if (colliderRef) colliderRef.enabled = active;
            if (collider2DRef) collider2DRef.enabled = active;
            if (renderers != null)
            {
                foreach (var r in renderers) if (r) r.enabled = active;
            }
        }
    }
}
