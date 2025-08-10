// ------------------------------------------------------------------------------
// File: GameState.cs
// Description: Stores global state: mode (2D/3D), memory count/goal.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace MemorySketch
{
    public enum DimensionMode { Mode2D, Mode3D }

    public class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }

        [Header("State")]
        [SerializeField] private DimensionMode mode = DimensionMode.Mode2D;
        [SerializeField] private int memoryCount = 0;
        [SerializeField] private int memoryGoal = 3;

        public event Action<DimensionMode> OnModeChanged;
        public event Action<int,int> OnMemoryChanged;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public DimensionMode Mode => mode;
        public int MemoryCount => memoryCount;
        public int MemoryGoal => memoryGoal;

        public void SetMode(DimensionMode newMode)
        {
            if (mode == newMode) return;
            mode = newMode;
            OnModeChanged?.Invoke(mode);
        }

        public void SetMemoryGoal(int goal)
        {
            memoryGoal = Mathf.Max(0, goal);
            OnMemoryChanged?.Invoke(memoryCount, memoryGoal);
        }

        public void AddMemory(int amount = 1)
        {
            memoryCount = Mathf.Max(0, memoryCount + amount);
            OnMemoryChanged?.Invoke(memoryCount, memoryGoal);
        }

        public bool HasEnoughMemories() => memoryCount >= memoryGoal;
    }
}
