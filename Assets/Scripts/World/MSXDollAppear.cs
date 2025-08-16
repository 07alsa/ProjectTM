using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXDollAppear : MonoBehaviour
    {
        public void Appear(Transform where)
        {
            if (!where) return;
            transform.position = where.position;
            transform.rotation = where.rotation;
            gameObject.SetActive(true);
        }
    }

}
