// SketchLocal2DZone.cs (핵심 추가 부분만)
using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch
{
    public class SketchLocal2DZone : MonoBehaviour
    {
        [Header("2D 진입에 필요한 참조")]
        public Transform pictureAnchor;      // 그림 평면(벽/바닥). Z+ 가 평면의 '정면(바깥)'을 향하도록 회전만 맞춰주세요.
        public Transform spawnPoint2D;       // 진입 위치(앵커 자식으로 빈 오브젝트 하나 두는 걸 권장)
        public Transform player2D;           // 씬에 있는 2D 캐릭터(하나). 시작 시 비활성화 권장.
        public Transform player3D;           // 3D 캐릭터(복귀 시 카메라가 다시 따라갈 대상)

        [Header("2D 카메라 설정(앵커별)")]
        public float orthoSize = 6f;         // 2D 줌
        public float normalDistance = 6f;    // 평면에서 떨어질 거리(정면)
        public Vector2 xLimits = new Vector2(-999, 999); // X 이동 범위
        public bool followY = false;         // 필요 시 Y 추적
        public float yOffset = 0f;           // Y 오프셋
        public float followXSmooth = 10f;    // X 추적 부드러움

        [Header("Events")]
        public UnityEvent OnEnter2D;
        public UnityEvent OnExit2D;

        [Header("Colliders/Object Toggle (기존 필드 유지)")]
        public GameObject[] enableIn2D;
        public GameObject[] disableIn2D;
        public Collider[] colliders3D;
        public Collider2D[] colliders2D;

        bool _active;

        public void EnterLocal2D()
        {
            if (_active) return;
            _active = true;

            // 2D 오브젝트/콜라이더 토글
            SetActive(enableIn2D, true);
            SetActive(disableIn2D, false);
            SetEnabled(colliders3D, false);
            SetEnabled(colliders2D, true);

            // 2D 플레이어 스냅 & 활성
            if (player2D && pictureAnchor && spawnPoint2D)
            {
                // 그림 평면 정면/위쪽에 맞춰 2D 플레이어 정렬
                player2D.rotation = Quaternion.LookRotation(pictureAnchor.forward, pictureAnchor.up);
                player2D.position = spawnPoint2D.position;
                if (!player2D.gameObject.activeSelf) player2D.gameObject.SetActive(true);
            }

            // 카메라에 2D 앵커/플레이어 전달
            var cam = Camera.main ? Camera.main.GetComponent<CameraController>() : null;
            if (cam)
            {
                cam.Set2DAnchor(pictureAnchor, player2D, orthoSize, normalDistance, xLimits, followY, yOffset, followXSmooth);
            }

            // 모드 전환: GameState만 사용
            if (GameState.Instance != null) GameState.Instance.SetMode(DimensionMode.Mode2D);

            OnEnter2D?.Invoke();
        }

        public void ExitLocal2D()
        {
            if (!_active) return;
            _active = false;

            SetActive(enableIn2D, false);
            SetActive(disableIn2D, true);
            SetEnabled(colliders3D, true);
            SetEnabled(colliders2D, false);

            // 2D 플레이어 숨김(원하면 유지해도 됨)
            if (player2D) player2D.gameObject.SetActive(false);

            // 카메라 2D 앵커 해제
            var cam = Camera.main ? Camera.main.GetComponent<CameraController>() : null;
            if (cam) cam.Clear2DAnchor();

            if (GameState.Instance != null) GameState.Instance.SetMode(DimensionMode.Mode3D);

            OnExit2D?.Invoke();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            EnterLocal2D();
        }

        // --- 유틸 ---
        static void SetActive(GameObject[] arr, bool on) { if (arr == null) return; foreach (var go in arr) if (go) go.SetActive(on); }
        static void SetEnabled(Collider[] arr, bool on) { if (arr == null) return; foreach (var c in arr) if (c) c.enabled = on; }
        static void SetEnabled(Collider2D[] arr, bool on) { if (arr == null) return; foreach (var c in arr) if (c) c.enabled = on; }
    }
}
