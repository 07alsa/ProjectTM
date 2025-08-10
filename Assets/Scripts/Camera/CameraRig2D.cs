// ------------------------------------------------------------------------------
// File: CameraRig2D.cs
// Description: 2D camera helper (optional placeholder).
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    public class CameraRig2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

        private void LateUpdate()
        {
            if (!target) return;
            transform.position = target.position + offset;
            transform.rotation = Quaternion.identity;
        }
    }
}
