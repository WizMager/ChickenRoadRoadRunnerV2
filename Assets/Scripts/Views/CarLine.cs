using System;
using DG.Tweening;
using Services.Audio;
using UnityEngine;
using ESoundType = Db.Sound.ESoundType;

namespace Views
{
    public class CarLine : MonoBehaviour
    {
        public Action<bool> OnSafeZone;
        
        //car
        [SerializeField] private SpriteRenderer _car;
        [SerializeField] private float _startMoveYPosition = 12f;
        [SerializeField] private float _stopMoveYPosition = 4.5f;
        //barrier
        [SerializeField] private SpriteRenderer _barrier;
        [SerializeField] private float _roadBarrierYPosition = 1.45f;

        private Tween _tween;
        private bool _isCarMove;
        private bool _isFirstPartOfPathDone;
        private bool _isSecondPartOfPathDone;
        private AudioService _audioService;
        private bool _hasPlayedPassingSound;


        public void Initialize(AudioService audioService)
        {
            _audioService = audioService;
        }
        
        public void StartCar(float fullRunTime, Sprite carIcon)
        {
            
        }

        public void StopCar(float fullRunTime)
        {
           
        }

        public void ResetState()
        {
            
        }
    }
}