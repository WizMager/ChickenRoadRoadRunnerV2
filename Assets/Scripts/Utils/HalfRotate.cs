using System;
using UnityEngine;

namespace Utils
{
    public class HalfRotate : StateMachineBehaviour
    {
        public Action OnRotate;

        private bool _isSended;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isSended = false;
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime >= 0.1f && !_isSended)
            {
                _isSended = true;
                OnRotate?.Invoke();
            }
        }
    }
}