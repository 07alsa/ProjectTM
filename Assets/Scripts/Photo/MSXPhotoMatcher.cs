using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXPhotoMatcher : MonoBehaviour
    {
        public Transform requiredPose;
        [Range(0f,1f)] public float angleDotThreshold = 0.97f;
        public float holdTime = 2.0f;
        public UnityEvent OnMatched;

        float _t;
        bool _fired;

        void Update()
        {
            if (_fired) return;
            var cam = Camera.main;
            if (!cam || !requiredPose) { _t = 0f; return; }
            float dot = Vector3.Dot(cam.transform.forward, requiredPose.forward);
            if (dot >= angleDotThreshold)
            {
                _t += Time.deltaTime;
                if (_t >= holdTime) { OnMatched?.Invoke(); _fired = true; }
            }
            else _t = 0f;
        }

        public void ForceMatch() => OnMatched?.Invoke();
    }

}
