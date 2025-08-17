// OneCameraBrain.cs
using UnityEngine;

namespace MemorySketch
{
    public enum OneCamMode { Mode3D, Mode2D }

    /// ���� ī�޶� �ϳ��� 3D/2D�� ��ȯ�ϸ� ����/�����̹�
    [RequireComponent(typeof(Camera))]
    public class OneCameraBrain : MonoBehaviour
    {
        [Header("Common")]
        public Transform yawPivot;    // (����) 3D���� ���콺 ���� ���� Yaw/Pitch ������ ������ �� �� ����
        public Transform pitchPivot;  // (����)
        public float transitionSeconds = 0.35f;

        [Header("3D Follow")]
        public Transform follow3D;          // 3D���� ���� ���(3D Player)
        public Vector3 follow3DOffset = new Vector3(0, 1.6f, -3.0f);
        public float follow3DSmooth = 10f;
        public float perspectiveFov = 60f;

        [Header("2D Follow")]
        public Transform follow2D;          // 2D���� ���� ���(2D Player)
        public Camera2DAnchor anchor2D;
        public float enter2DHeightLerp = 0.2f; // 2D ���� �� ī�޶� ���� ���� ����ġ

        Camera _cam;
        OneCamMode _mode = OneCamMode.Mode3D;
        Vector3 _vel; // SmoothDamp��
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

            // Ÿ�� ���� ���� ���� ������
            Vector3 targetPos = follow3D.position + follow3D.TransformVector(follow3DOffset);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _vel, 1f / Mathf.Max(0.01f, follow3DSmooth));

            // FOV ����(��ȯ �� �ܿ� ����)
            if (!_cam.orthographic)
                _cam.fieldOfView = Mathf.SmoothDamp(_cam.fieldOfView, perspectiveFov, ref _fovVel, 0.15f);
        }

        // ---------------- 2D ----------------
        void Tick2D()
        {
            if (!anchor2D || !follow2D) return;

            // ��Ŀ ���� ��ǥ��
            Vector3 right = anchor2D.transform.right;     // 2D X
            Vector3 up = anchor2D.transform.up;        // 2D Y
            Vector3 normal = anchor2D.transform.forward;   // ��� ����(�׸� �ٱ�����)

            // �÷��̾ ��Ŀ ��ǥ�� ����
            Vector3 d = follow2D.position - anchor2D.transform.position;
            float x = Vector3.Dot(d, right);
            float y = Vector3.Dot(d, up);

            // X ����/����
            x = Mathf.Clamp(x, anchor2D.xLimits.x, anchor2D.xLimits.y);
            float yGoal = anchor2D.followY ? y : anchor2D.yOffset;

            // ī�޶� ��ǥ ��ġ = ��� ���� normal �Ÿ���ŭ ��������, X/Y�� ���� �̵�
            Vector3 goal = anchor2D.transform.position
                         + right * x
                         + up * Mathf.Lerp(anchor2D.yOffset, yGoal, enter2DHeightLerp)
                         - normal * anchor2D.normalDistance;

            transform.position = Vector3.SmoothDamp(transform.position, goal, ref _vel, 1f / Mathf.Max(0.01f, anchor2D.followXSmooth));

            // �׸� ������ ���ϵ��� ȸ��
            Quaternion look = Quaternion.LookRotation(normal, up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 10f);

            // Orthographic ������ ����
            if (_cam.orthographic)
                _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, anchor2D.orthoSize, Time.deltaTime * 8f);
        }

        // ---------------- PUBLIC API ----------------

        /// 3D ���� ��ȯ (��� ���� �� �ε巴�� ����)
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

        /// 2D ���� ��ȯ (�׸� ����/Orthographic ����)
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

            // ���� ���� �׸� ������ ���ϰ� ��� �� �� ����(�ʱ� ƽ ������ �ȵ� �� �����Ƿ�)
            if (anchor2D)
            {
                Vector3 normal = anchor2D.transform.forward;
                Vector3 up = anchor2D.transform.up;
                transform.rotation = Quaternion.LookRotation(normal, up);

                Vector3 right = anchor2D.transform.right;
                float x = 0f; // ���� �� �߾ӿ��� ����
                Vector3 start = anchor2D.transform.position
                              + right * x
                              + up * anchor2D.yOffset
                              - normal * anchor2D.normalDistance;
                transform.position = Vector3.Lerp(transform.position, start, transitionSeconds > 0f ? 0.6f : 1f);
            }
        }
    }
}
