// ------------------------------------------------------------------------------
// File: Player2DController.cs
// Description: Handles 2D movement (Rigidbody2D or Transform).
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    [RequireComponent(typeof(Collider2D))]
    public class Player2DController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private bool allowVertical = false;

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = allowVertical ? Input.GetAxisRaw("Vertical") : 0f;
            Vector2 move = new Vector2(h, v).normalized;
            transform.Translate(move * moveSpeed * Time.deltaTime);
        }
    }
}
