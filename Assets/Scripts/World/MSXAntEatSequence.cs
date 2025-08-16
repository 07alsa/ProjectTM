using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch.Addons
{

    public class MSXAntEatSequence : MonoBehaviour
    {
        public GameObject keyPrefab;
        public Transform keySpawnPoint;
        public UnityEvent OnBegin;
        public UnityEvent OnEnd;

        public GameObject PlayAndSpawnKey()
        {
            OnBegin?.Invoke();
            // TODO: 애니메이션 타이밍/사운드 연결
            OnEnd?.Invoke();

            if (!keyPrefab) return null;
            return Instantiate(keyPrefab, keySpawnPoint ? keySpawnPoint.position : transform.position,
                                         keySpawnPoint ? keySpawnPoint.rotation : Quaternion.identity);
        }
    }

}
