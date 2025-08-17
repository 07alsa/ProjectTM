// ------------------------------------------------------------------------------
// File: SketchPortalExit.cs
// Purpose: Exit from local 2D mode when player enters this trigger.
// ------------------------------------------------------------------------------
using UnityEngine;

namespace MemorySketch
{
    public class SketchPortalExit : MonoBehaviour
    {
        public SketchLocal2DZone targetZone;
        public string requiredTag = "Player";

        void OnTriggerEnter(Collider other)
        {
            if (!targetZone) return;
            if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag)) return;
            targetZone.ExitLocal2D();
        }
    }
}
