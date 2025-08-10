using UnityEngine;

namespace MemorySketch
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Header("Targets")]
        public Transform target2D;
        public Transform target3D;

        [Header("2D (Orthographic)")]
        public float orthoSize = 6f;
        public float z2D = -10f;            // 2D일 때 카메라 z 위치
        public float followLerp2D = 12f;

        [Header("3D (Perspective)")]
        public float fov3D = 60f;
        public Vector3 offset3D = new Vector3(0, 5, -8);
        public float followLerp3D = 8f;
        public bool lookAtTarget3D = true;

        Camera _cam;
        ViewMode _mode = ViewMode.Mode2D;

        void Awake()
        {
            _cam = GetComponent<Camera>();
            ApplyMode(_mode, instant: true);
        }

        void LateUpdate()
        {
            if (_mode == ViewMode.Mode2D && target2D)
            {
                var pos = new Vector3(target2D.position.x, target2D.position.y, z2D);
                transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * followLerp2D);
                transform.rotation = Quaternion.identity;
            }
            else if (_mode == ViewMode.Mode3D && target3D)
            {
                var pos = target3D.position + offset3D;
                transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * followLerp3D);
                if (lookAtTarget3D)
                {
                    var rot = Quaternion.LookRotation(target3D.position - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * followLerp3D);
                }
            }
        }

        public void Set2D(bool instant = false) => ApplyMode(ViewMode.Mode2D, instant);
        public void Set3D(bool instant = false) => ApplyMode(ViewMode.Mode3D, instant);

        public void ApplyMode(ViewMode mode, bool instant)
        {
            _mode = mode;

            if (_mode == ViewMode.Mode2D)
            {
                _cam.orthographic = true;
                _cam.orthographicSize = orthoSize;
                if (instant && target2D)
                {
                    transform.position = new Vector3(target2D.position.x, target2D.position.y, z2D);
                    transform.rotation = Quaternion.identity;
                }
            }
            else
            {
                _cam.orthographic = false;
                _cam.fieldOfView = fov3D;
                if (instant && target3D)
                {
                    transform.position = target3D.position + offset3D;
                    if (lookAtTarget3D) transform.LookAt(target3D);
                }
            }
        }
    }
}
