using Db;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ui
{
    public class MinigamePopupWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _content;
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _heartImage;
        [SerializeField] private RectTransform _heartTargetPoint;
        [SerializeField] private RectTransform _heartHomePoint;

        private UiData _uiData;
        private Sequence _contentSequence;
        private Sequence _heartSequence;

        public void Initialize(UiData uiData)
        {
            _uiData = uiData;
        }
        
        private void Start()
        {
            _animator.GetBehaviour<WheelSpinAnimationSignal>().OnSignal += OnWheelSpinAnimationSignal;

            _canvasGroup.blocksRaycasts = true;
            PrepareState();
            PlayShowAnimation();
        }

        private void OnWheelSpinAnimationSignal()
        {
            MoveHeartToWindow();
        }

        private void OnDisable()
        {
            _contentSequence?.Kill();
            _heartSequence?.Kill();
        }

        private void PrepareState()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }

            if (_content != null)
            {
                _content.localScale = Vector3.zero;
            }

            if (_heartImage != null)
            {
                _heartImage.enabled = false;
                if (_heartHomePoint != null)
                {
                    _heartImage.rectTransform.position = _heartHomePoint.position;
                }
            }
        }

        private void PlayShowAnimation()
        {
            _contentSequence?.Kill();
            _contentSequence = DOTween.Sequence();

            if (_canvasGroup != null)
            {
                _contentSequence.Append(_canvasGroup.DOFade(1f, _uiData.MinigameStartAppearDuration));
            }

            if (_content != null)
            {
                _contentSequence.Join(_content.DOScale(Vector3.one, _uiData.MinigameContentScaleDuration)
                    .SetEase(Ease.OutBack));
            }

            _contentSequence.OnComplete(() =>
            {
                _animator.SetTrigger("Play");
            });
        }

        private void MoveHeartToWindow()
        {
            if (_heartImage == null || _heartTargetPoint == null || _heartHomePoint == null)
            {
                CloseWindow();
                return;
            }
            
            _heartImage.enabled = true;
            _heartImage.rectTransform.position = _heartTargetPoint.position;

            _heartSequence?.Kill();
            _heartSequence = DOTween.Sequence();
            
            _heartSequence.AppendInterval(_uiData.DurationBeforeHearthTravel);
            _heartSequence.Append(_heartImage.rectTransform.DOMove(_heartHomePoint.position, _uiData.HeartTravelDuration)
                .SetEase(Ease.InOutSine));
            _heartSequence.OnComplete(CloseWindow);
        }

        private void CloseWindow()
        {
            if (_canvasGroup == null)
            {
                gameObject.SetActive(false);
                return;
            }

            _canvasGroup.DOFade(0f, _uiData.CloseFadeDuration)
                .OnComplete(() =>
                {
                    _canvasGroup.interactable = false;
                    _canvasGroup.blocksRaycasts = false;
                    gameObject.SetActive(false);
                    _canvasGroup.blocksRaycasts = false;
                });
        }
    }
}

