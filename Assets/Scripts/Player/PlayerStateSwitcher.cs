using UnityEngine;

namespace MemorySketch
{
    public class PlayerStateSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject player2D;
        [SerializeField] private GameObject player3D;

        // CameraController 참조/호출은 이제 필요 X
        // [SerializeField] private CameraController cameraController;  // ← 제거

        // CameraModeBinder가 읽어갈 공개 속성
        public ViewMode CurrentMode { get; private set; } = ViewMode.Mode2D;

        private void Start()
        {
            // 당신의 GameState 흐름 유지
            ApplyMode(GameState.Instance.Mode);
            GameState.Instance.OnModeChanged += ApplyMode;
        }

        private void OnDestroy()
        {
            if (GameState.Instance != null)
                GameState.Instance.OnModeChanged -= ApplyMode;
        }

        public void ToggleMode()
        {
            var next = GameState.Instance.Mode == DimensionMode.Mode2D
                ? DimensionMode.Mode3D
                : DimensionMode.Mode2D;

            GameState.Instance.SetMode(next);
        }

        // GameState에서 내려오는 DimensionMode를 ViewMode로 변환해서 보관
        private void ApplyMode(DimensionMode mode)
        {
            bool to3D = (mode == DimensionMode.Mode3D);

            if (player2D) player2D.SetActive(!to3D);
            if (player3D) player3D.SetActive(to3D);

            // 카메라는 여기서 건드리지 않음!
            CurrentMode = to3D ? ViewMode.Mode3D : ViewMode.Mode2D;
        }
    }
}
