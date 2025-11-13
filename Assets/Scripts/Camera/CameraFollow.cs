using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float leftBound = -10f;
        [SerializeField] private float rightBound = 10f;
        [SerializeField] private float playerOffset;
        [SerializeField] private float smoothSpeed = 5f;
    
        private UnityEngine.Camera _camera;
        private bool _isFollowing;

        public bool BlockCamera;

        public void Initialize(UnityEngine.Camera camera)
        {
            _camera = camera;
        }
    
        private void LateUpdate()
        {
            if (BlockCamera)
                return;
            
            if (player == null) return;
        
            var cameraHalfWidth = GetCameraHalfWidth();
            var targetX = transform.position.x;
            var playerScreenPos = _camera.WorldToViewportPoint(player.position).x;
        
            if (playerScreenPos > 0.5f + playerOffset)
            {
                _isFollowing = true;
            }
        
            if (_isFollowing)
            {
                targetX = player.position.x - (0.5f + playerOffset) * cameraHalfWidth * 2f;
            }
        
            var minX = leftBound + cameraHalfWidth;
            var maxX = rightBound - cameraHalfWidth;
            targetX = Mathf.Clamp(targetX, minX, maxX);
        
            var newX = Mathf.Lerp(transform.position.x, targetX, smoothSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    
        private float GetCameraHalfWidth()
        {
            return _camera.orthographicSize * _camera.aspect;
        }
    }
}