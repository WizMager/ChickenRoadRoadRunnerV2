using Db;
using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject _text;

        private Sequence _sequence;
        private IconsData _iconsData;

        private void Initialize(IconsData iconsData)
        {
            _iconsData = iconsData;
        }
        
        public void Stay()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            var time = 1f;
            _sequence.Append(_spriteRenderer.transform.DORotate(new Vector3(0, 90, 0), time));
            _sequence.Join(DOVirtual.Color(_spriteRenderer.color, new Color(1, 1, 1, 0), time / 3, value =>
            {
                _spriteRenderer.color = value;
            }));
        }

        public void JumpFrom()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _spriteRenderer.sprite = _iconsData.GetSewerSprite(false);

            var time = 1f;
            _sequence.Append(_spriteRenderer.transform.DORotate(new Vector3(0, 0, 0), time));
            _sequence.Insert(time / 3,DOVirtual.Color(_spriteRenderer.color, new Color(1, 1, 1, 1), time / 3 * 2, value =>
            {
                _spriteRenderer.color = value;
            }));
        }

        public void ResetState()
        {
            _spriteRenderer.sprite = _iconsData.GetSewerSprite(false);
            _spriteRenderer.color = Color.white;
            _spriteRenderer.transform.rotation = Quaternion.identity;
        }
    }
}