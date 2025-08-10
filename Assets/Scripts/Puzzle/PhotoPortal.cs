// ------------------------------------------------------------------------------
// File: PhotoPortal.cs
// Description: Enter to move through a photo/drawing portal. Supports same-scene teleport.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    public class PhotoPortal : MonoBehaviour
    {
        [Header("Teleport (Same Scene)")]
        [SerializeField] private Transform destinationPoint;
        [Header("Optional Scene Load")]
        [SerializeField] private string targetSceneName;
        [SerializeField] private bool useSceneLoad = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (useSceneLoad && !string.IsNullOrEmpty(targetSceneName))
            {
                SceneFlow.Instance.LoadScene(targetSceneName);
            }
            else if (destinationPoint)
            {
                other.transform.position = destinationPoint.position;
            }
        }
    }
}
