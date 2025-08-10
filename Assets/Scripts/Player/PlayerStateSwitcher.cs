using UnityEngine;

namespace MemorySketch
{
    public class PlayerStateSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject player2D;
        [SerializeField] private GameObject player3D;

        // CameraController ����/ȣ���� ���� �ʿ� X
        // [SerializeField] private CameraController cameraController;  // �� ����

        // CameraModeBinder�� �о ���� �Ӽ�
        public ViewMode CurrentMode { get; private set; } = ViewMode.Mode2D;

        private void Start()
        {
            // ����� GameState �帧 ����
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

        // GameState���� �������� DimensionMode�� ViewMode�� ��ȯ�ؼ� ����
        private void ApplyMode(DimensionMode mode)
        {
            bool to3D = (mode == DimensionMode.Mode3D);

            if (player2D) player2D.SetActive(!to3D);
            if (player3D) player3D.SetActive(to3D);

            // ī�޶�� ���⼭ �ǵ帮�� ����!
            CurrentMode = to3D ? ViewMode.Mode3D : ViewMode.Mode2D;
        }
    }
}
