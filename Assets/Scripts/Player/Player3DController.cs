using UnityEngine;

namespace MemorySketch
{
    [RequireComponent(typeof(CharacterController))]
    public class Player3DController : MonoBehaviour
    {
        [Header("Move")]
        public float moveSpeed = 6f;
        public float jumpHeight = 1.2f;
        public float gravity = -9.81f;

        [Header("Camera")]
        public Transform cam;  // ���� MainCamera �ڵ�

        [Header("Ground Check (Layer based)")]
        public Transform groundCheck;          // �߳� Transform
        public float groundRadius = 0.2f;      // ���� ���� �ݰ�
        public LayerMask groundLayers;         // �ٴ� ���̾� ����

        CharacterController cc;
        Vector3 velocity;

        void Awake()
        {
            cc = GetComponent<CharacterController>();
            if (!cam && Camera.main) cam = Camera.main.transform;
        }

        bool IsGrounded()
        {
            if (!groundCheck) return cc.isGrounded; // ���
            return Physics.CheckSphere(
                groundCheck.position,
                groundRadius,
                groundLayers,
                QueryTriggerInteraction.Ignore  // Ʈ���Ŵ� ����
            );
        }

        void Update()
        {
            // �Է�(�õ� ��ǲ): �� ���� �� Ű ����
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(h) < 0.01f) { if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h -= 1f; if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f; }
            if (Mathf.Abs(v) < 0.01f) { if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) v -= 1f; if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) v += 1f; }

            // ī�޶� ���� ��� �̵�
            Vector3 fwd = cam ? Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized : Vector3.forward;
            Vector3 right = cam ? Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized : Vector3.right;
            Vector3 move = (fwd * v + right * h);
            if (move.sqrMagnitude > 1f) move.Normalize();
            cc.Move(move * moveSpeed * Time.deltaTime);

            // ����/�߷� (���̾� ��� ����)
            bool grounded = IsGrounded();
            if (grounded && velocity.y < 0f) velocity.y = -2f;

            if (Input.GetButtonDown("Jump") && grounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            velocity.y += gravity * Time.deltaTime;
            cc.Move(velocity * Time.deltaTime);
        }

        void OnDrawGizmosSelected()
        {
            if (groundCheck)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
            }
        }
    }
}
