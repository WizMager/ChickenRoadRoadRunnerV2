using System;
using DG.Tweening;
using Services.Audio;
using UnityEngine;
using ESoundType = Db.Sound.ESoundType;

namespace Views
{
    public class CarLine : MonoBehaviour
    {
        //car
        [SerializeField] private SpriteRenderer _car;
        [SerializeField] private float _startMoveYPosition = 12f;
        [SerializeField] private float _endMoveYPosition = -12f;
        [SerializeField] private float _stopMoveYPosition = 4.5f;
        
        //barrier
        [SerializeField] private SpriteRenderer _barrier;
        [SerializeField] private SpriteRenderer _barrierShadow;
        [SerializeField] private float _roadBarrierYPosition = 1.68f;
        
        
        public void StartCar(float time, Sprite carIcon)
        {
            if (_car == null)
            {
                return;
            }
            
            if (time <= 0f)
            {
                time = 0.01f;
            }
            
            DOTween.Kill(_barrierShadow);

            _car.enabled = true;
            _car.sprite = carIcon;
            var position = _car.transform.position;
            position.y = _startMoveYPosition;
            _car.transform.position = position;
            
            _car.transform.DOMoveY(_stopMoveYPosition, time).SetEase(Ease.Linear);
        }

        public void StartBarrier(float time)
        {
            if (_barrier == null || _barrierShadow == null)
            {
                return;
            }

            if (time <= 0f)
            {
                time = 0.01f;
            }
            
            DOTween.Kill(_barrier.transform);
            DOTween.Kill(_barrierShadow);

            _barrier.enabled = true;
            _barrier.transform.DOMoveY(_roadBarrierYPosition, time).SetEase(Ease.Linear);

            var shadowDelay = time / 3f * 2;
            var shadowDuration = Math.Max(0.01f, time - shadowDelay);

            _barrierShadow.DOFade(1f, shadowDuration).SetDelay(shadowDelay).SetEase(Ease.Linear);
        }
        
        public void StartFullRunCar(float time, Sprite carIcon)
        {
            
        }

        public void ResetState()
        {
            
        }
    }
}