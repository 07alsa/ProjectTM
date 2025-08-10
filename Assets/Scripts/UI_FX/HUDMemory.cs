// ------------------------------------------------------------------------------
// File: HUDMemory.cs
// Description: Displays collected memory count (current/goal).
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace MemorySketch
{
    public class HUDMemory : MonoBehaviour
    {
        [SerializeField] private Text memoryText;

        private void Start()
        {
            if (GameState.Instance != null)
            {
                Refresh(GameState.Instance.MemoryCount, GameState.Instance.MemoryGoal);
                GameState.Instance.OnMemoryChanged += Refresh;
            }
        }

        private void OnDestroy()
        {
            if (GameState.Instance != null)
                GameState.Instance.OnMemoryChanged -= Refresh;
        }

        private void Refresh(int current, int goal)
        {
            if (memoryText) memoryText.text = $"Memories: {current} / {goal}";
        }
    }
}
