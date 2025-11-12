using UnityEngine;

namespace Camera
{
    public class CameraFollow : ICameraFollow
    {
        private readonly Transform _cameraTransform;
        private readonly Transform _targetTransform;
        private readonly float _offsetX;

        public CameraFollow(
            Transform cameraTransform, 
            Transform targetTransform, 
            float offsetX
        )
        {
            _cameraTransform = cameraTransform;
            _targetTransform = targetTransform;
            _offsetX = offsetX;
        }

        public void Update()
        {
            _cameraTransform.position = new Vector3(_targetTransform.position.x + _offsetX, _cameraTransform.position.y, _cameraTransform.position.z);
        }
    }
}