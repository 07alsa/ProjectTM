using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXPrefabSpawner : MonoBehaviour
    {
        public Transform spawnPoint;
        public GameObject Spawn(GameObject prefab)
        {
            if (!prefab) return null;
            return Instantiate(prefab, spawnPoint ? spawnPoint.position : transform.position,
                                        spawnPoint ? spawnPoint.rotation : Quaternion.identity);
        }
    }

}
