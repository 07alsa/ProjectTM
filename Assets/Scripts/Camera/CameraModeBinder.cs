using UnityEngine;

namespace MemorySketch
{
    // PlayerStateSwitcher에 public ViewMode CurrentMode { get; } 가 있다고 가정
    public class CameraModeBinder : MonoBehaviour
    {
        [Header("Refs")]
        public PlayerStateSwitcher switcher;
        public CameraController cam;

        ViewMode _last;

        void Start()
        {
            if (!switcher) switcher = FindObjectOfType<PlayerStateSwitcher>();
            if (!cam) cam = FindObjectOfType<CameraController>();
            Apply(instant: true);
        }

        void Update()
        {
            if (!switcher || !cam) return;
            if (switcher.CurrentMode != _last)
                Apply(instant: false);
        }

        void Apply(bool instant)
        {
            _last = switcher.CurrentMode;
            if (_last == ViewMode.Mode2D) cam.Set2D(instant);
            else cam.Set3D(instant);
        }
    }
}
