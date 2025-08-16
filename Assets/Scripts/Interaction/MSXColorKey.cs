using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXColorKey : MonoBehaviour
    {
        public string colorId = "Red";
        public StringEvent OnKeyPressed;

        public void Press()
        {
            OnKeyPressed?.Invoke(colorId);
        }
    }

}
