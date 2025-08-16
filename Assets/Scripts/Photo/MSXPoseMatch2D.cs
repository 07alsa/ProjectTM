using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    // 2D에서 일정 시간 포즈 유지시 발화
    public class MSXPoseMatch2D : MonoBehaviour
    {
        public float holdTime = 0.5f;
        public UnityEvent OnMatch;
        float _t;

        public void TickHold(float dt)
        {
            _t += dt;
            if (_t >= holdTime) { OnMatch?.Invoke(); _t = 0f; }
        }

        public void ResetHold() => _t = 0f;
        public void ForceMatch() => OnMatch?.Invoke();
    }

}
