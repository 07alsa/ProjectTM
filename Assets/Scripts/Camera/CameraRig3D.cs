// ------------------------------------------------------------------------------
// File: CameraRig3D.cs
// Description: 3D camera helper (follow & look at).
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    public class CameraRig3D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);

        private void LateUpdate()
        {
            if (!target) return;
            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, 0.15f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position, Vector3.up), 0.15f);
        }
    }
}
