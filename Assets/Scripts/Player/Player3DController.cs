using UnityEngine;

namespace MemorySketch
{
    [RequireComponent(typeof(CharacterController))]
    public class Player3DController : MonoBehaviour
    {
        public float moveSpeed = 6f;
        public float jumpHeight = 1.2f;
        public float gravity = -9.81f;
        public Transform cam;   // 비우면 자동으로 Main Camera

        private CharacterController cc;
        private Vector3 velocity;

        private void Awake()
        {
            cc = GetComponent<CharacterController>();
            if (!cam && Camera.main) cam = Camera.main.transform;
        }

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal"); // A/D, ←/→
            float v = Input.GetAxisRaw("Vertical");   // W/S, ↑/↓

            // 카메라 기준 평면 벡터
            Vector3 fwd = Vector3.ProjectOnPlane((cam ? cam.forward : Vector3.forward), Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane((cam ? cam.right : Vector3.right), Vector3.up).normalized;

            Vector3 move = (fwd * v + right * h);
            if (move.sqrMagnitude > 1f) move.Normalize();

            cc.Move(move * moveSpeed * Time.deltaTime);

            // 점프 & 중력
            if (cc.isGrounded && velocity.y < 0f) velocity.y = -2f;
            if (Input.GetButtonDown("Jump") && cc.isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            velocity.y += gravity * Time.deltaTime;
            cc.Move(velocity * Time.deltaTime);
        }
    }
}
