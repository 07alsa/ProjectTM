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
        [Tooltip("3D ��忡�� ���� ��� (3D �÷��̾�)")]
        public Transform follow3D;

        [Tooltip("��� ���� ������ (���� ����)")]
        public Vector3 follow3DOffset = new Vector3(0f, 1.6f, -3f);

        [Tooltip("��ġ ���� �ε巯�� (���� Ŭ���� �� ������ ����)")]
        [Range(0.01f, 30f)] public float follow3DSmooth = 10f;

        [Tooltip("3D ��� FOV")]
        public float perspectiveFov = 60f;

        private Vector3 _vel3D;   // 3D SmoothDamp��

        // ---------------- 2D SETTINGS ----------------
        [Header("2D Mode (runtime-set)")]
        [SerializeField] private bool is2D = false;

        [SerializeField, Tooltip("�׸� ��� Transform (Z+ �� ��� �ٱ��� ���ϵ��� ����)")]
        private Transform pictureAnchor;

        [SerializeField, Tooltip("2D ��忡�� ���� ��� (2D �÷��̾�)")]
        private Transform follow2D;

        [SerializeField, Tooltip("Orthographic ī�޶� ������ (2D ��)")]
        private float orthoSize = 6f;

        [SerializeField, Tooltip("�׸� ��鿡�� ������ �Ÿ�(���� ����)")]
        private float normalDistance = 6f;

        [SerializeField, Tooltip("2D���� ī�޶� ���� X ���� (��/��)")]
        private Vector2 xLimits = new Vector2(-999f, 999f);

        [SerializeField, Tooltip("2D���� Y�� ������ ���� (��κ��� ���δ� �� ����)")]
        private bool followY = false;

        [SerializeField, Tooltip("followY=false�� �� ���� Y ������")]
        private float yOffset = 0f;

        [SerializeField, Tooltip("2D X ���� �ε巯�� (���� Ŭ���� �� ������ ����)")]
        private float followXSmooth = 10f;

        private Vector3 _vel2D;   // 2D SmoothDamp��

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

            // GameState �̺�Ʈ ���� (������Ʈ�� �̹� �����Ѵٰ� ����)
            if (GameState.Instance != null)
                GameState.Instance.OnModeChanged += OnModeChanged;
        }

        private void OnDestroy()
        {
            if (GameState.Instance != null)
                GameState.Instance.OnModeChanged -= OnModeChanged;
        }

        // GameState���� ��尡 �ٲ� �� ȣ���
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

            // �ʱ� ���� (��Ŀ �߽�/�������� �� �� ����)
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

            // ��Ŀ�� �̹� ���õǾ� �ִٸ� ��� �� �� �� ����
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
                // ȸ���� �������� ����(���콺 ��/���װ� ����)
            }
        }

        // ----------------------------------------------------------------------

        private void LateUpdate()
        {
            if (!camera) return;

            // --------------- 2D ī�޶� �����̹� ---------------
            if (is2D && pictureAnchor && follow2D)
            {
                Vector3 right = pictureAnchor.right;    // 2D X
                Vector3 up = pictureAnchor.up;       // 2D Y
                Vector3 normal = pictureAnchor.forward;  // �׸� ����(�ٱ�)

                // 2D �÷��̾ ��Ŀ ��ǥ��� ����
                Vector3 d = follow2D.position - pictureAnchor.position;
                float x = Vector3.Dot(d, right);
                float y = Vector3.Dot(d, up);

                // X ���� + ��ǥ ��ġ ���
                x = Mathf.Clamp(x, xLimits.x, xLimits.y);
                float yGoal = followY ? y : yOffset;

                Vector3 goal = pictureAnchor.position
                             + right * x
                             + up * yGoal
                             - normal * normalDistance;

                // ��ġ ����
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    goal,
                    ref _vel2D,
                    1f / Mathf.Max(0.01f, followXSmooth)
                );

                // ȸ��: �׻� �׸� ������ ������
                Quaternion look = Quaternion.LookRotation(normal, up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    look,
                    Time.deltaTime * 10f
                );

                // Orthographic ������ ����(�ε巴��)
                camera.orthographicSize = Mathf.Lerp(
                    camera.orthographicSize,
                    orthoSize,
                    Time.deltaTime * 8f
                );

                return; // 2D ó�� �� 3D ������ �ǳʶ�
            }

            // --------------- 3D ī�޶� ���� ---------------
            if (follow3D)
            {
                // ��� ���� �������� ����� ��ȯ�ؼ� ���󰡱�
                Vector3 targetPos = follow3D.position + follow3D.TransformVector(follow3DOffset);

                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    targetPos,
                    ref _vel3D,
                    1f / Mathf.Max(0.01f, follow3DSmooth)
                );

                // ȸ���� ���⼭ �������� ���� (���콺 ��/ĳ���� ���װ� �����ϵ���)
            }
        }
    }
}
