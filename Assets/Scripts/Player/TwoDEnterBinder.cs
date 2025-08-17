// TwoDEnterBinder.cs
using UnityEngine;

namespace MemorySketch
{
    public class TwoDEnterBinder : MonoBehaviour
    {
        public Player2DService player2DService;
        public OneCameraBrain cameraBrain;

        public Camera2DAnchor anchor;
        public Transform spawnPoint;    // ���� anchor ������ "2DSpawn"

        // SketchLocal2DZone.OnEnter2D �� ����
        public void OnEnter2D()
        {
            var t2d = player2DService.SpawnAt(anchor, spawnPoint);
            if (t2d) cameraBrain.To2D(anchor, t2d);
        }

        // SketchLocal2DZone.OnExit2D �� ����
        public void OnExit2D(Transform player3D)
        {
            player2DService.Hide();
            cameraBrain.To3D(player3D);
        }
    }
}
