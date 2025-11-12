using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class RoadBarrier : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _roadBarrier;
        [SerializeField] private float _roadBarrierYPosition = 1.45f;
        
        private float _startBarrierYPosition;
        private Tween _tween;
        
        private void Start()
        {
            _startBarrierYPosition = _roadBarrier.transform.position.y;
        }

        public void MoveBarrier(float timeToDownBarrier)
        {
            _roadBarrier.enabled = true;
            
            _tween = _roadBarrier.transform.DOMoveY(_roadBarrierYPosition, timeToDownBarrier);
        }

        public bool IsBarrierDown => _roadBarrier.enabled && Mathf.Abs(_roadBarrier.transform.position.y - _roadBarrierYPosition) < 0.1f;

        public void ResetState()
        {
            _tween?.Kill();
            
            _roadBarrier.enabled = false;
            var position = _roadBarrier.transform.position;
            position.y = _startBarrierYPosition;
            _roadBarrier.transform.position = position;
        }
    }
}