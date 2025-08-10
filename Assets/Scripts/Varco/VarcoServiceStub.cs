// ------------------------------------------------------------------------------
// File: VarcoServiceStub.cs
// Description: Stub service that maps SketchData to prefabs (simulates VARCO 3D).
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemorySketch
{
    [System.Serializable]
    public class ShapePrefab
    {
        public SketchShapeType shape;
        public GameObject prefab;
    }

    public class VarcoServiceStub : MonoBehaviour
    {
        [Header("Prefabs by Shape")]
        [SerializeField] private List<ShapePrefab> prefabs = new List<ShapePrefab>();
        [SerializeField, Range(0f, 2f)] private float simulateDelay = 0.25f;

        public IEnumerator RequestSpawn(SketchData data, System.Action<GameObject> onReady)
        {
            // Simulate async processing
            yield return new WaitForSeconds(simulateDelay);
            GameObject prefab = ResolvePrefab(data.shape);
            onReady?.Invoke(prefab);
        }

        private GameObject ResolvePrefab(SketchShapeType shape)
        {
            foreach (var sp in prefabs)
                if (sp.shape == shape) return sp.prefab;
            return null;
        }
    }
}
