using Db;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Views
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _text;

        private Sequence _sequence;
        private IconsData _iconsData;
        private GameData _gameData;

        public void Initialize(
            IconsData iconsData, 
            string text,
            GameData gameData
        )
        {
            _iconsData = iconsData;
            _text.text = text;
            _gameData = gameData;
        }
        
        public void Stay()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            var time = _gameData.TimeToStepMove;
            _sequence.Append(_spriteRenderer.transform.DORotate(new Vector3(0, 90, 0), time));
            _sequence.Join(DOVirtual.Color(_spriteRenderer.color, new Color(1, 1, 1, 0), time / 3, value =>
            {
                _spriteRenderer.color = value;
            }));
            _sequence.Join(_canvasGroup.DOFade(0, time / 3));
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