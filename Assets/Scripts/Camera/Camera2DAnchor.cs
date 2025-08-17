// Camera2DAnchor.cs
using UnityEngine;

namespace MemorySketch
{
    /// 2D 모드에서 카메라가 참조할 "그림 평면" 앵커
    /// - transform.forward : 평면의 법선(그림에서 바깥쪽을 향하도록 세팅)
    /// - transform.right   : 2D에서 "가로(X)" 축
    /// - transform.up      : 2D에서 "세로(Y)" 축
    public class Camera2DAnchor : MonoBehaviour
    {
        [Header("2D Camera Framing")]
        [Tooltip("그림 평면에서 떨어질 거리(법선 방향).")]
        public float normalDistance = 6f;

        [Tooltip("Orthographic 카메라 사이즈.")]
        public float orthoSize = 6f;

        [Tooltip("2D에서 카메라가 따라갈 X 범위(그림 내부 한계를 줄 때 사용). x=좌, y=우")]
        public Vector2 xLimits = new Vector2(-999f, 999f);

        [Tooltip("2D에서 카메라가 따라갈 Y 보정(필요하면 그림 중앙에서 상/하로 살짝 치우치기).")]
        public float yOffset = 0f;

        [Header("Follow")]
        [Tooltip("X축 따라가기 부드러움")]
        public float followXSmooth = 10f;

        [Tooltip("Y축을 따라갈지 여부(대부분 2D 플랫포머는 고정 권장)")]
        public bool followY = false;

        [Tooltip("followY=true일 때 쓰는 부드러움")]
        public float followYSmooth = 8f;
    }
}
