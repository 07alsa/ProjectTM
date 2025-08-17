// Player2DService.cs
using UnityEngine;

namespace MemorySketch
{
    public class Player2DService : MonoBehaviour
    {
        [Header("Prefab")]
        public GameObject player2DPrefab;

        [Header("Runtime")]
        public GameObject instance;     // ���� ����ִ� 2D ĳ����
        public Transform instanceRoot;  // ������ �ڵ� ����

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

            // �׸� ��ǥ�迡 �°� ���� ���� (X=�¿�, Y=��/�Ʒ�, Z=ȭ�� ��)
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
