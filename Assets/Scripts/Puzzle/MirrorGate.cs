// ------------------------------------------------------------------------------
// File: MirrorGate.cs
// Description: Opens when required memories are collected.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    public class MirrorGate : MonoBehaviour
    {
        [SerializeField] private int requiredMemories = 3;
        [SerializeField] private Animator animator;
        private bool opened = false;

        private void Start()
        {
            GameState.Instance.OnMemoryChanged += OnMemoryChanged;
            OnMemoryChanged(GameState.Instance.MemoryCount, GameState.Instance.MemoryGoal);
        }

        private void OnDestroy()
        {
            if (GameState.Instance != null)
                GameState.Instance.OnMemoryChanged -= OnMemoryChanged;
        }

        private void OnMemoryChanged(int current, int goal)
        {
            if (opened) return;
            if (current >= requiredMemories)
            {
                Open();
            }
        }

        private void Open()
        {
            opened = true;
            if (animator) animator.SetTrigger("Open");
            // TODO: Enable portal collider or trigger next scene
        }
    }
}
