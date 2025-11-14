using System;
using UnityEngine;

namespace Utils
{
    public class ChickenAnimationState : StateMachineBehaviour
    {
        public Action OnAnimationEnd;
        
        [SerializeField]
        [Range(0f, 1f)]
        private float _threshold = 0.85f;
        
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
            OnAnimationEnd?.Invoke();
        }
    }
}