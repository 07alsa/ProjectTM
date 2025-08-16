using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXSnakeJarOpen : MonoBehaviour
    {
        public Animator animator;
        public UnityEvent OnOpen;

        public void Open()
        {
            if (animator) animator.SetTrigger("Open");
            OnOpen?.Invoke();
        }
    }

}
