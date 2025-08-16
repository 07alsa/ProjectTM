using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    // 간단 오버레이 토글(실제 구현은 UI Image/Blit로 연동)
    public class MSXPhotoPoseOverlay : MonoBehaviour
    {
        public Texture overlayTexture;
        [Range(0f,1f)] public float overlayAlpha = 0.35f;
        public UnityEvent OnEnableOverlay;
        public UnityEvent OnDisableOverlay;

        public void EnableOverlay()  { OnEnableOverlay?.Invoke(); }
        public void DisableOverlay() { OnDisableOverlay?.Invoke(); }
    }

}
