using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _leftBound = -10f;
        [SerializeField] private float _rightBound = 10f;
        [SerializeField] private float _playerOffset;
        [SerializeField] private float _smoothSpeed = 5f;
    
        private UnityEngine.Camera _camera;
        private bool _isFollowing;
        public bool BlockCamera;
        
        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
        }
    
        private void LateUpdate()
        {
            if (BlockCamera)
                return;
            
            if (_target == null) return;
        
            var cameraHalfWidth = GetCameraHalfWidth();
            var targetX = transform.position.x;
            var playerScreenPos = _camera.WorldToViewportPoint(_target.position).x;
        
            if (playerScreenPos > 0.5f + _playerOffset)
            {
                _isFollowing = true;
            }
        
            if (_isFollowing)
            {
                targetX = _target.position.x - (0.5f + _playerOffset) * cameraHalfWidth * 2f;
            }
        
            var minX = _leftBound + cameraHalfWidth;
            var maxX = _rightBound - cameraHalfWidth;
            targetX = Mathf.Clamp(targetX, minX, maxX);
        
            var newX = Mathf.Lerp(transform.position.x, targetX, _smoothSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    
        private float GetCameraHalfWidth()
        {
            return _camera.orthographicSize * _camera.aspect;
        }
    }
}