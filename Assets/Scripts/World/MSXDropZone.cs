using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXDropZone : MonoBehaviour
    {
        public string acceptsItemId = "Candy";
        public UnityEvent OnDropAccepted;

        public void Drop(string itemId)
        {
            if (itemId == acceptsItemId) OnDropAccepted?.Invoke();
        }
    }

}
