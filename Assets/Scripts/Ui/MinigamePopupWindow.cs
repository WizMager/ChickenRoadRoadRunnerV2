using Db;
using Db.Sound;
using DG.Tweening;
using Services.Audio;
using TMPro;
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
        [SerializeField] private TMP_Text _dailyBonusFirstText;
        [SerializeField] private TMP_Text _dailyBonusSecondText;
        [SerializeField] private TMP_Text _getText;
        [SerializeField] private Button _getButton;

        [LunaPlaygroundField("Title 1 text", 1, "Minigame Window")]
        public string DailyBonusFirstText;
        
        [LunaPlaygroundField("Title 2 text", 1, "Minigame Window")]
        public string DailyBonusSecondText;
        
        [LunaPlaygroundField("Get text", 2, "Minigame Window")]
        public string GetText;
        
        private UiData _uiData;
        private AudioService _audioService;
        private Sequence _contentSequence;
        private Sequence _heartSequence;
        private bool _isSpinStarted;

        public void Initialize(
            UiData uiData,
            AudioService audioService
        )
        {
            _uiData = uiData;
            _audioService = audioService;
        }

        private void Awake()
        {
            _dailyBonusFirstText.text = DailyBonusFirstText;
            _dailyBonusSecondText.text = DailyBonusSecondText;
            _getText.text = GetText;
        }

        private void Start()
        {
            var behaviour = _animator.GetBehaviour<WheelSpinAnimationSignal>();
            behaviour.OnSignal += OnWheelSpinAnimationSignal;
            behaviour.OnSpinStart += OnWheelSpinStart;
            behaviour.OnSpinEnd += OnWheelSpinEnd;
            
            _getButton.onClick.AddListener(OnGetBonus);

            _canvasGroup.blocksRaycasts = true;
            PrepareState();
            PlayShowAnimation();
        }
        
        private void OnWheelSpinStart()
        {
            if (_isSpinStarted)
                return;

            _isSpinStarted = true;

            
        }

        private void OnWheelSpinEnd()
        {
            _audioService.StopSound();
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
                _getButton.interactable = true;
            });
        }

        private void OnGetBonus()
        {
            _audioService.PlaySound(ESoundType.Wheel);
            _animator.SetTrigger("Play");
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

