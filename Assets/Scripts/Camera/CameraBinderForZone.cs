// CameraBinderForZone.cs
using UnityEngine;

namespace MemorySketch
{
    /// SketchLocal2DZone ���� Ʈ������ UnityEvent�� �����ؼ� ī�޶� ��ȯ
    public class CameraBinderForZone : MonoBehaviour
    {
        public OneCameraBrain cameraBrain;

        [Header("Targets")]
        public Transform player3D;
        public Transform player2D;
        public Camera2DAnchor anchor2D;

        // SketchLocal2DZone.OnEnter2D �� ����
        public void OnEnter2D()
        {
            if (cameraBrain && anchor2D && player2D)
                cameraBrain.To2D(anchor2D, player2D);
        }

        // SketchLocal2DZone.OnExit2D �� ����
        public void OnExit2D()
        {
            if (cameraBrain && player3D)
                cameraBrain.To3D(player3D);
        }
    }
}
