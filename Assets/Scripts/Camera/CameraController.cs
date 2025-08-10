// ------------------------------------------------------------------------------
// File: CameraController.cs
// Description: Simple camera follow for 2D/3D modes.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    public class CameraController : MonoBehaviour
    {
        [Header("Targets")]
        [SerializeField] private Transform target2D;
        [SerializeField] private Transform target3D;

        [Header("Offsets")]
        [SerializeField] private Vector3 offset2D = new Vector3(0, 0, -10);
        [SerializeField] private Vector3 offset3D = new Vector3(0, 5, -10);

        private bool is3D = false;

        private void LateUpdate()
        {
            Transform t = is3D ? target3D : target2D;
            if (!t) return;
            Vector3 desired = t.position + (is3D ? offset3D : offset2D);
            transform.position = Vector3.Lerp(transform.position, desired, 0.2f);
            if (is3D) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(t.position - transform.position, Vector3.up), 0.15f);
            else transform.rotation = Quaternion.identity;
        }

        public void SwitchCamera(bool to3D) => is3D = to3D;
    }
}
