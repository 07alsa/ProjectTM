using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXDoorOpener : MonoBehaviour
    {
        public Animator animator;
        public UnityEvent OnUnlocked;
        public bool openOnce = true;
        bool _opened;

        public void Unlock()
        {
            if (openOnce && _opened) return;
            _opened = true;
            if (animator) animator.SetTrigger("Open");
            OnUnlocked?.Invoke();
        }
    }

}
