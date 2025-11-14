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
        [SerializeField] private float _roadBarrierYPosition = 1.45f;
        
        
        public void StartCar(float time, Sprite carIcon)
        {
            
        }

        public void StartBarrier()
        {
            
        }
        
        public void StartFullRunCar(float time, Sprite carIcon)
        {
            
        }

        public void ResetState()
        {
            
        }
    }
}