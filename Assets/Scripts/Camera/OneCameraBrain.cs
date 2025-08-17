// OneCameraBrain.cs
using UnityEngine;

namespace MemorySketch
{
    public enum OneCamMode { Mode3D, Mode2D }

    /// 메인 카메라 하나로 3D/2D를 전환하며 추적/프레이밍
    [RequireComponent(typeof(Camera))]
    public class OneCameraBrain : MonoBehaviour
    {
        [Header("Common")]
        public Transform yawPivot;    // (선택) 3D에서 마우스 룩을 쓰면 Yaw/Pitch 리그의 기준이 될 수 있음
        public Transform pitchPivot;  // (선택)
        public float transitionSeconds = 0.35f;

        [Header("3D Follow")]
        public Transform follow3D;          // 3D에서 따라갈 대상(3D Player)
        public Vector3 follow3DOffset = new Vector3(0, 1.6f, -3.0f);
        public float follow3DSmooth = 10f;
        public float perspectiveFov = 60f;

        [Header("2D Follow")]
        public Transform follow2D;          // 2D에서 따라갈 대상(2D Player)
        public Camera2DAnchor anchor2D;
        public float enter2DHeightLerp = 0.2f; // 2D 진입 시 카메라 높이 보정 가중치

        Camera _cam;
        OneCamMode _mode = OneCamMode.Mode3D;
        Vector3 _vel; // SmoothDamp용
        float _fovVel;

        void Awake()
        {
            _cam = GetComponent<Camera>();
            if (_cam) _cam.orthographic = false;
            if (_cam) _cam.fieldOfView = perspectiveFov;
        }

        void LateUpdate()
        {
            if (_mode == OneCamMode.Mode3D)
            {
                Tick3D();
            }
            else
            {
                Tick2D();
            }
        }

        // ---------------- 3D ----------------
        void Tick3D()
        {
            if (!follow3D)
                return;

            // 타겟 기준 월드 공간 오프셋
            Vector3 targetPos = follow3D.position + follow3D.TransformVector(follow3DOffset);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _vel, 1f / Mathf.Max(0.01f, follow3DSmooth));

            // FOV 보간(전환 중 잔여 보정)
            if (!_cam.orthographic)
                _cam.fieldOfView = Mathf.SmoothDamp(_cam.fieldOfView, perspectiveFov, ref _fovVel, 0.15f);
        }

        // ---------------- 2D ----------------
        void Tick2D()
        {
            if (!anchor2D || !follow2D) return;

            // 앵커 기준 좌표축
            Vector3 right = anchor2D.transform.right;     // 2D X
            Vector3 up = anchor2D.transform.up;        // 2D Y
            Vector3 normal = anchor2D.transform.forward;   // 평면 법선(그림 바깥으로)

            // 플레이어를 앵커 좌표로 투영
            Vector3 d = follow2D.position - anchor2D.transform.position;
            float x = Vector3.Dot(d, right);
            float y = Vector3.Dot(d, up);

            // X 제한/보정
            x = Mathf.Clamp(x, anchor2D.xLimits.x, anchor2D.xLimits.y);
            float yGoal = anchor2D.followY ? y : anchor2D.yOffset;

            // 카메라 목표 위치 = 평면 앞쪽 normal 거리만큼 떨어지고, X/Y로 평행 이동
            Vector3 goal = anchor2D.transform.position
                         + right * x
                         + up * Mathf.Lerp(anchor2D.yOffset, yGoal, enter2DHeightLerp)
                         - normal * anchor2D.normalDistance;

            transform.position = Vector3.SmoothDamp(transform.position, goal, ref _vel, 1f / Mathf.Max(0.01f, anchor2D.followXSmooth));

            // 그림 정면을 향하도록 회전
            Quaternion look = Quaternion.LookRotation(normal, up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 10f);

            // Orthographic 사이즈 유지
            if (_cam.orthographic)
                _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, anchor2D.orthoSize, Time.deltaTime * 8f);
        }

        // ---------------- PUBLIC API ----------------

        /// 3D 모드로 전환 (즉시 세팅 후 부드럽게 보간)
        public void To3D(Transform newFollow3D = null)
        {
            if (newFollow3D) follow3D = newFollow3D;

            _mode = OneCamMode.Mode3D;
            if (_cam)
            {
                _cam.orthographic = false;
                _cam.fieldOfView = perspectiveFov;
            }
        }

        /// 2D 모드로 전환 (그림 정면/Orthographic 세팅)
        public void To2D(Camera2DAnchor anchor, Transform newFollow2D = null)
        {
            if (anchor) anchor2D = anchor;
            if (newFollow2D) follow2D = newFollow2D;

            _mode = OneCamMode.Mode2D;
            if (_cam)
            {
                _cam.orthographic = true;
                _cam.orthographicSize = anchor2D ? anchor2D.orthoSize : _cam.orthographicSize;
            }

            // 진입 순간 그림 정면을 향하게 즉시 한 번 맞춤(초기 틱 보장이 안될 수 있으므로)
            if (anchor2D)
            {
                Vector3 normal = anchor2D.transform.forward;
                Vector3 up = anchor2D.transform.up;
                transform.rotation = Quaternion.LookRotation(normal, up);

                Vector3 right = anchor2D.transform.right;
                float x = 0f; // 진입 시 중앙에서 시작
                Vector3 start = anchor2D.transform.position
                              + right * x
                              + up * anchor2D.yOffset
                              - normal * anchor2D.normalDistance;
                transform.position = Vector3.Lerp(transform.position, start, transitionSeconds > 0f ? 0.6f : 1f);
            }
        }
    }
}
