// ------------------------------------------------------------------------------
// File: MemoryShard.cs
// Description: Collectible memory shard that increments global count and can trigger a cutscene.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    public class MemoryShard : MonoBehaviour
    {
        [Header("Optional")]
        [SerializeField] private string cutsceneName;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            GameState.Instance.AddMemory(1);
            if (!string.IsNullOrEmpty(cutsceneName))
            {
                var player = FindObjectOfType<CutscenePlayer>();
                if (player) player.PlayCutscene(cutsceneName);
            }
            Destroy(gameObject);
        }
    }
}
