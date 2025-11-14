using System;
using UnityEngine;

namespace Utils
{
    public class ConfettiAnimationState : StateMachineBehaviour
    {
        public Action OnSignal;
        
        [SerializeField]
        [Range(0f, 1f)]
        private float _threshold = 0.7f;
        
        private bool _isSent;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isSent = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_isSent || stateInfo.normalizedTime < _threshold)
                return;
            
            _isSent = true;
            OnSignal?.Invoke();
        }
    }
}