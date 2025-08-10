// ------------------------------------------------------------------------------
// File: SketchSystem.cs
// Description: Captures simple sketch input and submits to Varco service.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MemorySketch
{
    public enum SketchShapeType { Line, Box, Arch, Custom }

    public struct SketchData
    {
        public SketchShapeType shape;
        public List<Vector3> points;
        public Vector3 spawnPosition;
        public Quaternion spawnRotation;
    }

    public class SketchSystem : MonoBehaviour
    {
        public event Action<SketchData> OnSketchSubmitted;

        [Header("Input")]
        [SerializeField] private KeyCode submitKey = KeyCode.Return;

        private List<Vector3> _points = new List<Vector3>();
        private SketchShapeType _shape = SketchShapeType.Line;

        public void BeginSketch(SketchShapeType shape)
        {
            _shape = shape;
            _points.Clear();
        }

        public void AddPoint(Vector3 worldPoint) => _points.Add(worldPoint);

        public void EndSketch(Vector3 spawnPos, Quaternion spawnRot)
        {
            var data = new SketchData { shape = _shape, points = new List<Vector3>(_points), spawnPosition = spawnPos, spawnRotation = spawnRot };
            OnSketchSubmitted?.Invoke(data);
            _points.Clear();
        }

        private void Update()
        {
            // Example: left click adds point, Enter submits with current transform
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 100f))
                {
                    AddPoint(hit.point);
                }
            }
            if (Input.GetKeyDown(submitKey))
            {
                EndSketch(transform.position, transform.rotation);
            }
        }
    }
}
