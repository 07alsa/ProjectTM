using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    // 포털 표면 오브젝트 활성화/Enter 이벤트만 제공 (타 시스템과 느슨 결합)
    public class MSXPhotoPortalLink : MonoBehaviour
    {
        public GameObject portalSurface;
        public UnityEvent OnActivated;
        public UnityEvent OnEnter;

        public void Activate(GameObject surface = null)
        {
            if (surface) portalSurface = surface;
            if (portalSurface) portalSurface.SetActive(true);
            OnActivated?.Invoke();
        }

        public void Enter()
        {
            OnEnter?.Invoke();
        }
    }

}
