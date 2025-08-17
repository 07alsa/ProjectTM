// CameraBinderForZone.cs
using UnityEngine;

namespace MemorySketch
{
    /// SketchLocal2DZone 같은 트리거의 UnityEvent에 연결해서 카메라를 전환
    public class CameraBinderForZone : MonoBehaviour
    {
        public OneCameraBrain cameraBrain;

        [Header("Targets")]
        public Transform player3D;
        public Transform player2D;
        public Camera2DAnchor anchor2D;

        // SketchLocal2DZone.OnEnter2D 에 연결
        public void OnEnter2D()
        {
            if (cameraBrain && anchor2D && player2D)
                cameraBrain.To2D(anchor2D, player2D);
        }

        // SketchLocal2DZone.OnExit2D 에 연결
        public void OnExit2D()
        {
            if (cameraBrain && player3D)
                cameraBrain.To3D(player3D);
        }
    }
}
