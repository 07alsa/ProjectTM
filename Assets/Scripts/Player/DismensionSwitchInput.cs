using UnityEngine;
using MemorySketch;

namespace MemorySketch
{
    public class DimensionSwitchInput : MonoBehaviour
    {
        [Header("Key Binding")]
        [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

        [Header("Refs")]
        [SerializeField] private PlayerStateSwitcher switcher;

        private void Reset()
        {
            // 같은 오브젝트/씬에서 자동으로 찾아 연결 시도
            if (!switcher) switcher = FindObjectOfType<PlayerStateSwitcher>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                if (!switcher) switcher = FindObjectOfType<PlayerStateSwitcher>();
                if (switcher) switcher.ToggleMode();
                else Debug.LogWarning("[DimensionSwitchInput] PlayerStateSwitcher가 씬에 없습니다.");
            }
        }
    }
}
