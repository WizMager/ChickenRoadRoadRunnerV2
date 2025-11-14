using System;
using UnityEngine;

namespace Utils
{
    public class ChickenAnimationState : StateMachineBehaviour
    {
        public Action OnAnimationEnd;
        
        private bool _isSent;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isSent = false;
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_isSent)
            {
                OnAnimationEnd?.Invoke();
                
                _isSent = true;
            }
        }
    }
}