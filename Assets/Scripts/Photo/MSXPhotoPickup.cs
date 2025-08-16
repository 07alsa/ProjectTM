using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXPhotoPickup : MonoBehaviour
    {
        public string itemId = "Candy";
        public StringEvent OnPicked;

        public void Pick()
        {
            OnPicked?.Invoke(itemId);
            gameObject.SetActive(false);
        }
    }

}
