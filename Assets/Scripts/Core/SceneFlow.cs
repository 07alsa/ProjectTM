// ------------------------------------------------------------------------------
// File: SceneFlow.cs
// Description: Handles scene loading/unloading and transitions.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MemorySketch
{
    public class SceneFlow : MonoBehaviour
    {
        public static SceneFlow Instance { get; private set; }

        [SerializeField] private CanvasGroup fadeCanvas;
        [SerializeField, Range(0.05f, 2f)] private float fadeDuration = 0.35f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string sceneName) => StartCoroutine(LoadSceneRoutine(sceneName));
        public void ReloadCurrentScene() => LoadScene(SceneManager.GetActiveScene().name);

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            yield return Fade(1f);
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            yield return Fade(0f);
        }

        public IEnumerator Fade(float target)
        {
            if (!fadeCanvas) yield break;
            fadeCanvas.blocksRaycasts = true;
            float start = fadeCanvas.alpha;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / fadeDuration;
                fadeCanvas.alpha = Mathf.Lerp(start, target, t);
                yield return null;
            }
            fadeCanvas.alpha = target;
            fadeCanvas.blocksRaycasts = target > 0.99f;
        }
    }
}
