// ------------------------------------------------------------------------------
// File: GameBoot.cs
// Description: Bootstraps the game and loads the initial scene.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MemorySketch
{
    public class GameBoot : MonoBehaviour
    {
        [Header("Initial Scene")]
        [SerializeField] private string initialSceneName = "Hub";

        private void Start()
        {
            StartCoroutine(BootRoutine());
        }

        private IEnumerator BootRoutine()
        {
            // TODO: Initialize services, save data, etc.
            yield return null;
            if (!string.IsNullOrEmpty(initialSceneName))
            {
                yield return SceneManager.LoadSceneAsync(initialSceneName, LoadSceneMode.Single);
            }
        }
    }
}
