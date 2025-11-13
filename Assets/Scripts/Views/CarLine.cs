using System;
using Db;
using Db.Sound;
using DG.Tweening;
using Services.Audio;
using UnityEngine;

namespace Views
{
    public class CarLine : MonoBehaviour
    {
        public Action<bool> OnSafeZone;
        
        [SerializeField] private SpriteRenderer _car;
        [SerializeField] private float _startMoveYPosition = 12f;
        [SerializeField] private float _stopMoveYPosition = 4.5f;
        [SerializeField] private float _safeZoneMoveYPosition = -2.25f;
        [SerializeField] private float _endMoveYPosition = -12f;

        private Tween _tween;
        private bool _isCarMove;
        private bool _isFirstPartOfPathDone;
        private bool _isSecondPartOfPathDone;
        private AudioService _audioService;
        private bool _hasPlayedPassingSound;
        private RoadBarrier _roadBarrier;
        
        public bool IsCarStopped { get; private set; }

        public void Initialize(AudioService audioService, RoadBarrier roadBarrier = null)
        {
            _audioService = audioService;
            _roadBarrier = roadBarrier;
        }
        
        public void StartCar(float fullRunTime, Sprite carIcon)
        {
            if (IsCarStopped)
                return;
            
            _tween?.Kill();
            
            _car.enabled = true;
            _car.sprite = carIcon;
            _hasPlayedPassingSound = false;

            OnSafeZone?.Invoke(true);
            _isCarMove = true;
            
            _tween = _car.transform.DOMoveY(_endMoveYPosition, fullRunTime).SetEase(Ease.Linear).OnUpdate(() =>
            {
				if (!_isFirstPartOfPathDone && _car.transform.position.y <= _stopMoveYPosition)
                {
                    _isFirstPartOfPathDone = true;
                    OnSafeZone?.Invoke(false);
                    
                    if (!_hasPlayedPassingSound && (_roadBarrier == null || !_roadBarrier.IsBarrierDown))
                    {
                        _audioService?.PlaySound(ESoundType.CarPassing);
                        _hasPlayedPassingSound = true;
                    }
                }
				else if (_isFirstPartOfPathDone && !_isSecondPartOfPathDone && _car.transform.position.y <= _safeZoneMoveYPosition)
                {
                    _isSecondPartOfPathDone = true;
                    OnSafeZone?.Invoke(true);
                }
                
            }).OnComplete(() =>
            {
                _isFirstPartOfPathDone = false;
                _isSecondPartOfPathDone = false;
                _car.enabled = false;
                var position = _car.transform.position;
                position.y = _startMoveYPosition;
                _car.transform.position = position;
                _isCarMove = false;
                _hasPlayedPassingSound = false;
            });
        }

        public void StopCar(float fullRunTime)
        {
            IsCarStopped = true;

            if (!_isCarMove || _isSecondPartOfPathDone)
                return;
            
            var elapsedTime = _tween.Elapsed();
            
            _tween.Kill();
            _audioService?.PlaySound(ESoundType.CarStop);
            
            _car.transform.DOMoveY(_stopMoveYPosition, fullRunTime / 3 - elapsedTime).OnComplete(() =>
            {
                _isFirstPartOfPathDone = false;
                _isSecondPartOfPathDone = false;
                _isCarMove = false;
            });
        }

        public void ResetState()
        {
            _tween?.Kill();
            
            _car.enabled = false;
            IsCarStopped = false;
            _isCarMove = false;
            _isFirstPartOfPathDone = false;
            _isSecondPartOfPathDone = false;
            
            var position = _car.transform.position;
            position.y = _startMoveYPosition;
            _car.transform.position = position;
        }
    }
}