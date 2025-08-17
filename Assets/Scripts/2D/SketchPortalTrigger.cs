// ------------------------------------------------------------------------------
// File: SketchPortalTrigger.cs
// Purpose: When the player enters this trigger, it calls EnterLocal2D() on a target SketchLocal2DZone.
//          Optional exit trigger to call ExitLocal2D().
// ------------------------------------------------------------------------------
using UnityEngine;

namespace MemorySketch
{
    public class SketchPortalTrigger : MonoBehaviour
    {
        public SketchLocal2DZone targetZone;
        public string requiredTag = "Player";

        void OnTriggerEnter(Collider other)
        {
            if (!targetZone) return;
            if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag)) return;
            targetZone.EnterLocal2D();
        }
    }
}
