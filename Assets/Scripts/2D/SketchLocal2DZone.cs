// SketchLocal2DZone.cs (�ٽ� �߰� �κи�)
using UnityEngine;
using UnityEngine.Events;

namespace MemorySketch
{
    public class SketchLocal2DZone : MonoBehaviour
    {
        [Header("2D ���Կ� �ʿ��� ����")]
        public Transform pictureAnchor;      // �׸� ���(��/�ٴ�). Z+ �� ����� '����(�ٱ�)'�� ���ϵ��� ȸ���� �����ּ���.
        public Transform spawnPoint2D;       // ���� ��ġ(��Ŀ �ڽ����� �� ������Ʈ �ϳ� �δ� �� ����)
        public Transform player2D;           // ���� �ִ� 2D ĳ����(�ϳ�). ���� �� ��Ȱ��ȭ ����.
        public Transform player3D;           // 3D ĳ����(���� �� ī�޶� �ٽ� ���� ���)

        [Header("2D ī�޶� ����(��Ŀ��)")]
        public float orthoSize = 6f;         // 2D ��
        public float normalDistance = 6f;    // ��鿡�� ������ �Ÿ�(����)
        public Vector2 xLimits = new Vector2(-999, 999); // X �̵� ����
        public bool followY = false;         // �ʿ� �� Y ����
        public float yOffset = 0f;           // Y ������
        public float followXSmooth = 10f;    // X ���� �ε巯��

        [Header("Events")]
        public UnityEvent OnEnter2D;
        public UnityEvent OnExit2D;

        [Header("Colliders/Object Toggle (���� �ʵ� ����)")]
        public GameObject[] enableIn2D;
        public GameObject[] disableIn2D;
        public Collider[] colliders3D;
        public Collider2D[] colliders2D;

        bool _active;

        public void EnterLocal2D()
        {
            if (_active) return;
            _active = true;

            // 2D ������Ʈ/�ݶ��̴� ���
            SetActive(enableIn2D, true);
            SetActive(disableIn2D, false);
            SetEnabled(colliders3D, false);
            SetEnabled(colliders2D, true);

            // 2D �÷��̾� ���� & Ȱ��
            if (player2D && pictureAnchor && spawnPoint2D)
            {
                // �׸� ��� ����/���ʿ� ���� 2D �÷��̾� ����
                player2D.rotation = Quaternion.LookRotation(pictureAnchor.forward, pictureAnchor.up);
                player2D.position = spawnPoint2D.position;
                if (!player2D.gameObject.activeSelf) player2D.gameObject.SetActive(true);
            }

            // ī�޶� 2D ��Ŀ/�÷��̾� ����
            var cam = Camera.main ? Camera.main.GetComponent<CameraController>() : null;
            if (cam)
            {
                cam.Set2DAnchor(pictureAnchor, player2D, orthoSize, normalDistance, xLimits, followY, yOffset, followXSmooth);
            }

            // ��� ��ȯ: GameState�� ���
            if (GameState.Instance != null) GameState.Instance.SetMode(DimensionMode.Mode2D);

            OnEnter2D?.Invoke();
        }

        public void ExitLocal2D()
        {
            if (!_active) return;
            _active = false;

            SetActive(enableIn2D, false);
            SetActive(disableIn2D, true);
            SetEnabled(colliders3D, true);
            SetEnabled(colliders2D, false);

            // 2D �÷��̾� ����(���ϸ� �����ص� ��)
            if (player2D) player2D.gameObject.SetActive(false);

            // ī�޶� 2D ��Ŀ ����
            var cam = Camera.main ? Camera.main.GetComponent<CameraController>() : null;
            if (cam) cam.Clear2DAnchor();

            if (GameState.Instance != null) GameState.Instance.SetMode(DimensionMode.Mode3D);

            OnExit2D?.Invoke();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            EnterLocal2D();
        }

        // --- ��ƿ ---
        static void SetActive(GameObject[] arr, bool on) { if (arr == null) return; foreach (var go in arr) if (go) go.SetActive(on); }
        static void SetEnabled(Collider[] arr, bool on) { if (arr == null) return; foreach (var c in arr) if (c) c.enabled = on; }
        static void SetEnabled(Collider2D[] arr, bool on) { if (arr == null) return; foreach (var c in arr) if (c) c.enabled = on; }
    }
}
