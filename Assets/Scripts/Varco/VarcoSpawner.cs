// ------------------------------------------------------------------------------
// File: VarcoSpawner.cs
// Description: Listens to SketchSystem and spawns prefabs via VarcoServiceStub.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using UnityEngine;

namespace MemorySketch
{
    public class VarcoSpawner : MonoBehaviour
    {
        [SerializeField] private SketchSystem sketchSystem;
        [SerializeField] private VarcoServiceStub varco;

        private void OnEnable()
        {
            if (sketchSystem != null)
                sketchSystem.OnSketchSubmitted += HandleSketchSubmitted;
        }

        private void OnDisable()
        {
            if (sketchSystem != null)
                sketchSystem.OnSketchSubmitted -= HandleSketchSubmitted;
        }

        private void HandleSketchSubmitted(SketchData data)
        {
            if (!varco) return;
            StartCoroutine(varco.RequestSpawn(data, prefab =>
            {
                if (prefab)
                {
                    Instantiate(prefab, data.spawnPosition, data.spawnRotation);
                }
                else
                {
                    Debug.LogWarning("VarcoServiceStub returned no prefab for shape: " + data.shape);
                }
            }));
        }
    }
}
