using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXPrinterOutput : MonoBehaviour
    {
        public Transform exitPoint;
        public GameObject Spawn(GameObject printPrefab)
        {
            if (!printPrefab) return null;
            return Instantiate(printPrefab, exitPoint ? exitPoint.position : transform.position,
                                          exitPoint ? exitPoint.rotation : Quaternion.identity);
        }
    }

}
