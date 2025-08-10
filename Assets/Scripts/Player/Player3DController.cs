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
        public Transform cam;  // 비우면 MainCamera 자동

        [Header("Ground Check (Layer based)")]
        public Transform groundCheck;          // 발끝 Transform
        public float groundRadius = 0.2f;      // 접지 판정 반경
        public LayerMask groundLayers;         // 바닥 레이어 선택

        CharacterController cc;
        Vector3 velocity;

        void Awake()
        {
            cc = GetComponent<CharacterController>();
            if (!cam && Camera.main) cam = Camera.main.transform;
        }

        bool IsGrounded()
        {
            if (!groundCheck) return cc.isGrounded; // 백업
            return Physics.CheckSphere(
                groundCheck.position,
                groundRadius,
                groundLayers,
                QueryTriggerInteraction.Ignore  // 트리거는 무시
            );
        }

        void Update()
        {
            // 입력(올드 인풋): 축 없을 때 키 폴백
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(h) < 0.01f) { if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h -= 1f; if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f; }
            if (Mathf.Abs(v) < 0.01f) { if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) v -= 1f; if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) v += 1f; }

            // 카메라 기준 평면 이동
            Vector3 fwd = cam ? Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized : Vector3.forward;
            Vector3 right = cam ? Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized : Vector3.right;
            Vector3 move = (fwd * v + right * h);
            if (move.sqrMagnitude > 1f) move.Normalize();
            cc.Move(move * moveSpeed * Time.deltaTime);

            // 점프/중력 (레이어 기반 접지)
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
