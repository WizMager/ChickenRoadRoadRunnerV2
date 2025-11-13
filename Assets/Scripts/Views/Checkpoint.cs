using UnityEngine;

namespace Views
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject _text;

        public void EnableText()
        {
            _text.SetActive(true);
        }
        
        public void DisableText()
        {
            _text.SetActive(false);
        }
    }
}