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
            // ���� ������Ʈ/������ �ڵ����� ã�� ���� �õ�
            if (!switcher) switcher = FindObjectOfType<PlayerStateSwitcher>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                if (!switcher) switcher = FindObjectOfType<PlayerStateSwitcher>();
                if (switcher) switcher.ToggleMode();
                else Debug.LogWarning("[DimensionSwitchInput] PlayerStateSwitcher�� ���� �����ϴ�.");
            }
        }
    }
}
