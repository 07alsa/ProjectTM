using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXColorPattern : MonoBehaviour
    {
        [Tooltip("정답 패턴 예) Red,Blue,Yellow")]
        public List<string> targetPattern = new List<string>();

        [Header("Timing")]
        public float maxIntervalBetweenNotes = 1.2f;

        [Header("Events")]
        public UnityEvent OnPatternMatched;
        public UnityEvent OnPatternFailed;

        private readonly List<string> buffer = new List<string>();
        private float lastTime = -999f;

        public void OnColorInput(string colorId)
        {
            float t = Time.time;
            if (t - lastTime > maxIntervalBetweenNotes) buffer.Clear();
            buffer.Add(colorId);
            lastTime = t;

            int n = buffer.Count;
            if (n > targetPattern.Count)
            {
                buffer.Clear();
                OnPatternFailed?.Invoke();
                return;
            }
            for (int i = 0; i < n; i++)
            {
                if (buffer[i] != targetPattern[i])
                {
                    buffer.Clear();
                    OnPatternFailed?.Invoke();
                    return;
                }
            }
            if (n == targetPattern.Count)
            {
                buffer.Clear();
                OnPatternMatched?.Invoke();
            }
        }
    }

}
