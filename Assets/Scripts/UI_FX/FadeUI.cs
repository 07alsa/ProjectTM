// ------------------------------------------------------------------------------
// File: FadeUI.cs
// Description: Controls CanvasGroup fade in/out.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace MemorySketch
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeUI : MonoBehaviour
    {
        private CanvasGroup cg;

        private void Awake() => cg = GetComponent<CanvasGroup>();

        public IEnumerator FadeTo(float target, float duration)
        {
            float start = cg.alpha;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / Mathf.Max(0.001f, duration);
                cg.alpha = Mathf.Lerp(start, target, t);
                yield return null;
            }
            cg.alpha = target;
            cg.blocksRaycasts = target > 0.99f;
        }
    }
}
