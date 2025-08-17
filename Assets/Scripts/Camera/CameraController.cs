// ------------------------------------------------------------------------------
// CameraController.cs  (Full replacement with Set2D/Set3D shims)
// - Single main camera for both 3D follow and 2D picture-framing.
// - 3D: Perspective, follow a 3D target with smoothing (rotation NOT forced).
// - 2D: Orthographic, align to a "picture plane" (anchor) and follow a 2D player
//       along the plane's X (and optionally Y), always looking straight at the plane.
// - Integrates with GameState.Instance.OnModeChanged (DimensionMode).
// - Adds Set2D(bool)/Set3D(bool) for compatibility with CameraModeBinder.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    public class CameraController : MonoBehaviour
    {
        // ---------------- 3D FOLLOW ----------------
        [Header("3D Follow")]
        [Tooltip("3D 모드에서 따라갈 대상 (3D 플레이어)")]
        public Transform follow3D;

        [Tooltip("대상 기준 오프셋 (로컬 기준)")]
        public Vector3 follow3DOffset = new Vector3(0f, 1.6f, -3f);

        [Tooltip("위치 추적 부드러움 (값이 클수록 더 빠르게 따라감)")]
        [Range(0.01f, 30f)] public float follow3DSmooth = 10f;

        [Tooltip("3D 모드 FOV")]
        public float perspectiveFov = 60f;

        private Vector3 _vel3D;   // 3D SmoothDamp용

        // ---------------- 2D SETTINGS ----------------
        [Header("2D Mode (runtime-set)")]
        [SerializeField] private bool is2D = false;

        [SerializeField, Tooltip("그림 평면 Transform (Z+ 가 평면 바깥을 향하도록 설정)")]
        private Transform pictureAnchor;

        [SerializeField, Tooltip("2D 모드에서 따라갈 대상 (2D 플레이어)")]
        private Transform follow2D;

        [SerializeField, Tooltip("Orthographic 카메라 사이즈 (2D 줌)")]
        private float orthoSize = 6f;

        [SerializeField, Tooltip("그림 평면에서 떨어질 거리(정면 방향)")]
        private float normalDistance = 6f;

        [SerializeField, Tooltip("2D에서 카메라가 따라갈 X 범위 (좌/우)")]
        private Vector2 xLimits = new Vector2(-999f, 999f);

        [SerializeField, Tooltip("2D에서 Y를 따라갈지 여부 (대부분은 꺼두는 것 권장)")]
        private bool followY = false;

        [SerializeField, Tooltip("followY=false일 때 고정 Y 오프셋")]
        private float yOffset = 0f;

        [SerializeField, Tooltip("2D X 추적 부드러움 (값이 클수록 더 빠르게 따라감)")]
        private float followXSmooth = 10f;

        private Vector3 _vel2D;   // 2D SmoothDamp용

        // ---------------- COMMON ----------------
        private new Camera camera;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            if (!camera) camera = Camera.main;

            if (camera)
            {
                camera.orthographic = false;
                camera.fieldOfView = perspectiveFov;
            }

            // GameState 이벤트 구독 (프로젝트에 이미 존재한다고 가정)
            if (GameState.Instance != null)
                GameState.Instance.OnModeChanged += OnModeChanged;
        }

        private void OnDestroy()
        {
            if (GameState.Instance != null)
                GameState.Instance.OnModeChanged -= OnModeChanged;
        }

        // GameState에서 모드가 바뀔 때 호출됨
        private void OnModeChanged(DimensionMode mode)
        {
            is2D = (mode == DimensionMode.Mode2D);

            if (!camera) return;

            if (is2D)
            {
                camera.orthographic = true;
                camera.orthographicSize = orthoSize;
            }
            else
            {
                camera.orthographic = false;
                camera.fieldOfView = perspectiveFov;
            }
        }

        // ----------------------------------------------------------------------
        // Public API used by SketchLocal2DZone to configure 2D anchor/follow
        // ----------------------------------------------------------------------
        public void Set2DAnchor(
            Transform anchor,
            Transform player2D,
            float ortho,
            float distance,
            Vector2 xLim,
            bool fY,
            float yOff,
            float xSmooth
        )
        {
            pictureAnchor = anchor;
            follow2D = player2D;
            orthoSize = ortho;
            normalDistance = distance;
            xLimits = xLim;
            followY = fY;
            yOffset = yOff;
            followXSmooth = xSmooth;

            if (camera)
            {
                camera.orthographic = true;
                camera.orthographicSize = orthoSize;
            }

            // 초기 스냅 (앵커 중심/정면으로 한 번 맞춤)
            if (pictureAnchor)
            {
                Vector3 right = pictureAnchor.right;
                Vector3 up = pictureAnchor.up;
                Vector3 normal = pictureAnchor.forward;

                float x = 0f;
                float y = yOffset;

                Vector3 start = pictureAnchor.position
                              + right * x
                              + up * y
                              - normal * normalDistance;

                transform.position = start;
                transform.rotation = Quaternion.LookRotation(normal, up);
            }
        }

        public void Clear2DAnchor()
        {
            pictureAnchor = null;
            follow2D = null;
        }

        // ----------------------------------------------------------------------
        // Compatibility shims for CameraModeBinder (expects Set2D/Set3D)
        // ----------------------------------------------------------------------
        public void Set2D(bool instant)
        {
            is2D = true;
            if (!camera) return;

            camera.orthographic = true;
            if (instant) camera.orthographicSize = orthoSize;

            // 앵커가 이미 세팅되어 있다면 즉시 한 번 더 스냅
            if (instant && pictureAnchor)
            {
                Vector3 right = pictureAnchor.right;
                Vector3 up = pictureAnchor.up;
                Vector3 normal = pictureAnchor.forward;

                float x = 0f;
                float y = yOffset;

                Vector3 start = pictureAnchor.position
                              + right * x
                              + up * y
                              - normal * normalDistance;

                transform.position = start;
                transform.rotation = Quaternion.LookRotation(normal, up);
            }
        }

        public void Set3D(bool instant)
        {
            is2D = false;
            if (!camera) return;

            camera.orthographic = false;
            if (instant) camera.fieldOfView = perspectiveFov;

            if (instant && follow3D)
            {
                Vector3 targetPos = follow3D.position + follow3D.TransformVector(follow3DOffset);
                transform.position = targetPos;
                // 회전은 강제하지 않음(마우스 룩/리그가 제어)
            }
        }

        // ----------------------------------------------------------------------

        private void LateUpdate()
        {
            if (!camera) return;

            // --------------- 2D 카메라 프레이밍 ---------------
            if (is2D && pictureAnchor && follow2D)
            {
                Vector3 right = pictureAnchor.right;    // 2D X
                Vector3 up = pictureAnchor.up;       // 2D Y
                Vector3 normal = pictureAnchor.forward;  // 그림 정면(바깥)

                // 2D 플레이어를 앵커 좌표계로 투영
                Vector3 d = follow2D.position - pictureAnchor.position;
                float x = Vector3.Dot(d, right);
                float y = Vector3.Dot(d, up);

                // X 제한 + 목표 위치 계산
                x = Mathf.Clamp(x, xLimits.x, xLimits.y);
                float yGoal = followY ? y : yOffset;

                Vector3 goal = pictureAnchor.position
                             + right * x
                             + up * yGoal
                             - normal * normalDistance;

                // 위치 보간
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    goal,
                    ref _vel2D,
                    1f / Mathf.Max(0.01f, followXSmooth)
                );

                // 회전: 항상 그림 정면을 보도록
                Quaternion look = Quaternion.LookRotation(normal, up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    look,
                    Time.deltaTime * 10f
                );

                // Orthographic 사이즈 유지(부드럽게)
                camera.orthographicSize = Mathf.Lerp(
                    camera.orthographicSize,
                    orthoSize,
                    Time.deltaTime * 8f
                );

                return; // 2D 처리 시 3D 로직은 건너뜀
            }

            // --------------- 3D 카메라 추적 ---------------
            if (follow3D)
            {
                // 대상 로컬 오프셋을 월드로 변환해서 따라가기
                Vector3 targetPos = follow3D.position + follow3D.TransformVector(follow3DOffset);

                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    targetPos,
                    ref _vel3D,
                    1f / Mathf.Max(0.01f, follow3DSmooth)
                );

                // 회전은 여기서 강제하지 않음 (마우스 룩/캐릭터 리그가 제어하도록)
            }
        }
    }
}
