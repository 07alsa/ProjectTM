// TwoDEnterBinder.cs
using UnityEngine;

namespace MemorySketch
{
    public class TwoDEnterBinder : MonoBehaviour
    {
        public Player2DService player2DService;
        public OneCameraBrain cameraBrain;

        public Camera2DAnchor anchor;
        public Transform spawnPoint;    // 보통 anchor 하위의 "2DSpawn"

        // SketchLocal2DZone.OnEnter2D 에 연결
        public void OnEnter2D()
        {
            var t2d = player2DService.SpawnAt(anchor, spawnPoint);
            if (t2d) cameraBrain.To2D(anchor, t2d);
        }

        // SketchLocal2DZone.OnExit2D 에 연결
        public void OnExit2D(Transform player3D)
        {
            player2DService.Hide();
            cameraBrain.To3D(player3D);
        }
    }
}
