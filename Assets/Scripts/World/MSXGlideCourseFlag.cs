using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXGlideCourseFlag : MonoBehaviour
    {
        public UnityEvent OnSuccess;
        public UnityEvent OnFail;

        public void CompleteSuccess() => OnSuccess?.Invoke();
        public void CompleteFail()    => OnFail?.Invoke();
    }

}
