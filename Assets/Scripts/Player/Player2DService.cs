// Player2DService.cs
using UnityEngine;

namespace MemorySketch
{
    public class Player2DService : MonoBehaviour
    {
        [Header("Prefab")]
        public GameObject player2DPrefab;

        [Header("Runtime")]
        public GameObject instance;     // 현재 살아있는 2D 캐릭터
        public Transform instanceRoot;  // 없으면 자동 생성

        void Awake()
        {
            if (!instanceRoot)
            {
                var go = new GameObject("Player2D_Root");
                instanceRoot = go.transform;
            }
        }

        public Transform SpawnAt(Camera2DAnchor anchor, Transform spawnPoint)
        {
            if (!player2DPrefab || !anchor || !spawnPoint) return null;

            if (!instance)
            {
                instance = Instantiate(player2DPrefab, instanceRoot);
            }

            // 그림 좌표계에 맞게 방향 정렬 (X=좌우, Y=위/아래, Z=화면 밖)
            instance.transform.rotation = Quaternion.LookRotation(anchor.transform.forward, anchor.transform.up);
            instance.transform.position = spawnPoint.position;

            instance.SetActive(true);
            return instance.transform;
        }

        public void Hide()
        {
            if (instance) instance.SetActive(false);
        }
    }
}
