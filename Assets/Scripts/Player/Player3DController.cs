// ------------------------------------------------------------------------------
// File: Player3DController.cs
// Description: Handles 3D movement using CharacterController.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    [RequireComponent(typeof(CharacterController))]
    public class Player3DController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float gravity = -9.81f;

        private CharacterController controller;
        private Vector3 velocity;

        private void Awake() => controller = GetComponent<CharacterController>();

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 move = new Vector3(h, 0f, v);
            controller.Move(move.normalized * moveSpeed * Time.deltaTime);

            if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
