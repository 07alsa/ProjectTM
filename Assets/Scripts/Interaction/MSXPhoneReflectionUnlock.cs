using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    // '움직임이 안정되면' 해제되는 간단 스테빌리티 감지
    public class MSXPhoneReflectionUnlock : MonoBehaviour
    {
        [Header("Stability")]
        public float posEpsilon = 0.0005f;
        public float rotEpsilon = 0.05f; // degrees
        public int stableFramesRequired = 20;

        [Header("Events")]
        public UnityEvent OnStable;

        Vector3 _lastPos;
        Quaternion _lastRot;
        bool _first = true;
        int _stable;

        void LateUpdate()
        {
            if (_first)
            {
                _first = false;
                _lastPos = transform.position;
                _lastRot = transform.rotation;
                _stable = 0;
                return;
            }

            float posDelta = (transform.position - _lastPos).sqrMagnitude;
            float rotDelta = Quaternion.Angle(transform.rotation, _lastRot);

            if (posDelta <= posEpsilon * posEpsilon && rotDelta <= rotEpsilon)
            {
                _stable++;
                if (_stable >= stableFramesRequired)
                {
                    OnStable?.Invoke();
                    enabled = false;
                }
            }
            else
            {
                _stable = 0;
            }

            _lastPos = transform.position;
            _lastRot = transform.rotation;
        }

        public void ForceStable() => OnStable?.Invoke();
    }

}
