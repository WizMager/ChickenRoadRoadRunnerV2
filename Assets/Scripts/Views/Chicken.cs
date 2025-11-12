using UnityEngine;

namespace Views
{
    public class Chicken : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        public void StartJumpAnimation()
        {
            _animator.SetTrigger("Jump");
        }
        
        public void InterruptJumpAnimation()
        {
            _animator.SetTrigger("InterruptJump");
        }
    }
}