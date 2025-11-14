using UnityEngine;

namespace Views
{
    public class Chicken : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _offsetY;

        public Animator GetAnimator => _animator;
        
        public void StartJumpAnimation()
        {
            _animator.SetTrigger("Jump");
        }
        
        public void InterruptJumpAnimation()
        {
            _animator.SetTrigger("InterruptJump");
        }

        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
        
        public void OffsetPosition(bool isEnable)
        {
            var position = transform.position;
            
            if (isEnable)
            {
                position.y += _offsetY;
            }
            else
            {
                position.y -= _offsetY;
            }
            
            transform.position = position;
            Debug.Log(transform.position);
        }
    }
}