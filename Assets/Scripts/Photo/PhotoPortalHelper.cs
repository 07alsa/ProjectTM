// ------------------------------------------------------------------------------
// File: PhotoPortalHelper.cs (FIXED)
// Purpose: Simple runtime helper to load the target photo scene.
//          Removed SerializedObject/UnityEditor dependency to avoid CS0246.
// ------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MemorySketch
{
    public class PhotoPortalHelper : MonoBehaviour
    {
        [Tooltip("Scene name to load when entering the photo portal")]
        public string targetSceneName;

        // Call this from a UnityEvent/Trigger/Button.
        public void LoadPhotoScene()
        {
            if (string.IsNullOrEmpty(targetSceneName))
            {
                Debug.LogWarning("[PhotoPortalHelper] targetSceneName is empty.");
                return;
            }
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
