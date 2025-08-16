using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    // 카메라가 reference와 각도/거리/시간 조건을 만족하면 OnAligned
    public class MSXFramingAligner : MonoBehaviour
    {
        [Header("Reference Pose")]
        public Transform reference;
        [Range(0f,1f)] public float angleDotThreshold = 0.96f;
        public float maxDistance = 3.0f;
        public float holdTime = 1.0f;
        public bool oneShot = true;

        [Header("Events")]
        public UnityEvent OnAligned;

        float _timer;
        bool _fired;

        void Update()
        {
            if (_fired && oneShot) return;
            var cam = Camera.main;
            if (!cam || !reference) { _timer = 0f; return; }

            float dot = Vector3.Dot(cam.transform.forward, reference.forward);
            float dist = Vector3.Distance(cam.transform.position, reference.position);

            if (dot >= angleDotThreshold && dist <= maxDistance)
            {
                _timer += Time.deltaTime;
                if (_timer >= holdTime)
                {
                    OnAligned?.Invoke();
                    _fired = true;
                }
            }
            else
            {
                _timer = 0f;
            }
        }

        public void ForceAligned()
        {
            if (_fired && oneShot) return;
            OnAligned?.Invoke();
            _fired = true;
        }
    }

}
