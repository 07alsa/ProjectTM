using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXMirrorRitual : MonoBehaviour
    {
        [Header("Dwell")]
        public float dwellSeconds = 4f;

        [Header("Framing")]
        [Range(0f,1f)] public float angleDotThreshold = 0.96f;
        public Transform framingReference;

        [Header("Light Step")]
        public float stepWindow = 1.0f;

        [Header("Events")]
        public UnityEvent OnRitualComplete;

        float _dwellT;
        bool _dwellOk, _frameOk, _stepOk;
        float _stepTimer;

        public void TickDwell(float dt)
        {
            if (_dwellOk) return;
            _dwellT += dt;
            if (_dwellT >= dwellSeconds) _dwellOk = true;
        }

        public void CheckFraming()
        {
            if (_frameOk) return;
            var cam = Camera.main;
            if (!cam || !framingReference) return;
            float dot = Vector3.Dot(cam.transform.forward, framingReference.forward);
            if (dot >= angleDotThreshold) _frameOk = true;
        }

        public void LightStep()
        {
            if (_stepOk) return;
            _stepOk = true;
        }

        void Update()
        {
            if (!_dwellOk) return;
            CheckFraming();
            if (_frameOk && _stepOk) OnRitualComplete?.Invoke();
            else if (_frameOk)
            {
                _stepTimer += Time.deltaTime;
                if (_stepTimer > stepWindow) { _frameOk = false; _stepTimer = 0f; }
            }
        }
    }

}
