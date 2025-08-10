using UnityEngine;

namespace MemorySketch
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player2DController : MonoBehaviour
    {
        [Header("Move & Jump")]
        public float moveSpeed = 6f;
        public float jumpForce = 12f;

        [Header("Ground Check")]
        public Transform groundCheck;         // �߳� �� ������Ʈ
        public float groundCheckRadius = 0.12f;
        public LayerMask groundLayers;

        private Rigidbody2D rb;
        private bool isGrounded;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            // 2D�� z=0 ��� ����, ȸ�� ����
            rb.freezeRotation = true;
        }

        private void Update()
        {
            // �¿� �̵� (x��)
            float h = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

            // ���� üũ
            if (groundCheck)
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);

            // ���� (y��)
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            // z�� �帮��Ʈ ����
            if (Mathf.Abs(transform.position.z) > 0.0001f)
            {
                var p = transform.position; p.z = 0f; transform.position = p;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
}
