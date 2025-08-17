// Camera2DAnchor.cs
using UnityEngine;

namespace MemorySketch
{
    /// 2D ��忡�� ī�޶� ������ "�׸� ���" ��Ŀ
    /// - transform.forward : ����� ����(�׸����� �ٱ����� ���ϵ��� ����)
    /// - transform.right   : 2D���� "����(X)" ��
    /// - transform.up      : 2D���� "����(Y)" ��
    public class Camera2DAnchor : MonoBehaviour
    {
        [Header("2D Camera Framing")]
        [Tooltip("�׸� ��鿡�� ������ �Ÿ�(���� ����).")]
        public float normalDistance = 6f;

        [Tooltip("Orthographic ī�޶� ������.")]
        public float orthoSize = 6f;

        [Tooltip("2D���� ī�޶� ���� X ����(�׸� ���� �Ѱ踦 �� �� ���). x=��, y=��")]
        public Vector2 xLimits = new Vector2(-999f, 999f);

        [Tooltip("2D���� ī�޶� ���� Y ����(�ʿ��ϸ� �׸� �߾ӿ��� ��/�Ϸ� ��¦ ġ��ġ��).")]
        public float yOffset = 0f;

        [Header("Follow")]
        [Tooltip("X�� ���󰡱� �ε巯��")]
        public float followXSmooth = 10f;

        [Tooltip("Y���� ������ ����(��κ� 2D �÷����Ӵ� ���� ����)")]
        public bool followY = false;

        [Tooltip("followY=true�� �� ���� �ε巯��")]
        public float followYSmooth = 8f;
    }
}
