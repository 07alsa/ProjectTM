using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXPaperPlaneRide : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;
        public float moveSeconds = 3f;

        public UnityEvent OnBegin;
        public UnityEvent OnSplash;
        public UnityEvent OnEnd;

        public void Begin()
        {
            OnBegin?.Invoke();
        }

        public void Splash() { OnSplash?.Invoke(); }
        public void EndRide() { OnEnd?.Invoke(); }
    }

}
