using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXKeyPoseAligner : MonoBehaviour
    {
        public Transform keyholeReference;
        [Range(0f,1f)] public float angleDotThreshold = 0.96f;
        public float holdTime = 0.8f;
        public UnityEvent OnAligned;

        float _t;

        void Update()
        {
            var cam = Camera.main;
            if (!cam || !keyholeReference) { _t = 0f; return; }
            float dot = Vector3.Dot(cam.transform.forward, keyholeReference.forward);
            if (dot >= angleDotThreshold)
            {
                _t += Time.deltaTime;
                if (_t >= holdTime) { OnAligned?.Invoke(); enabled = false; }
            }
            else _t = 0f;
        }

        public void ForceAligned() => OnAligned?.Invoke();
    }

}
