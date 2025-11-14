using System;
using System.Collections;
using Db;
using DG.Tweening;
using Services.Checkpoint;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Views;

namespace Ui
{
    public class GameHudWindow : MonoBehaviour
    {
        public Action OnNextPressed;
        public Action OnRevivePress;
        public Action OnWithdrawPress;
        
        [SerializeField] private Button _goNextButton;
        [SerializeField] private Button _withdrawButton;
        [SerializeField] private Button _reviveButton;

        [SerializeField] private TMP_Text _withdrawText;
        [SerializeField] private TMP_Text _goText;
        [SerializeField] private TMP_Text _reviveText;
        [SerializeField] private Image _reviveTutorArrow;
        [SerializeField] private Image _reviveHeartImage;
        
		//Balance
		[SerializeField] private TextMeshProUGUI _balanceText;
		[SerializeField] private TextMeshProUGUI _balanceValueText;
		
		[LunaPlaygroundField("Withdraw", 1, "Game HUD Window")]
		public string WithdrawText;
		
		[LunaPlaygroundField("Go", 2, "Game HUD Window")]
		public string GoText;
		
		[LunaPlaygroundField("Revive", 3, "Game HUD Window")]
		public string ReviveText;
		
		[LunaPlaygroundField("Balance value", 3, "Game HUD Window")]
		public string BalanceValueText;

		private UiData _uiData;
		private ICheckpointService _checkpointService;
		private GameData _gameData;
		private Chicken _chicken;
		private IconsData _iconsData;
		
        private Tween _arrowPulseTween;
        private Sequence _arrowSequence;
        private Vector2 _arrowStartAnchoredPosition;
        private bool _arrowStartCaptured;

        private float ArrowFadeDuration => _uiData != null ? _uiData.ReviveArrowFadeDuration : 0.25f;
        private float ArrowPulseDuration => _uiData != null ? _uiData.ReviveArrowPulseDuration : 0.6f;
        private float ArrowPulseScale => _uiData != null ? _uiData.ReviveArrowPulseScale : 1.2f;
        private float ArrowFlyOffDistance => _uiData != null ? _uiData.ReviveArrowFlyOffDistance : 400f;
        private float ArrowFlyOffDuration => _uiData != null ? _uiData.ReviveArrowFlyOffDuration : 0.35f;
        private Vector2 ArrowFlyOffDirection => _uiData != null ? _uiData.ReviveArrowFlyOffDirection : new Vector2(0f, 1f);
        private float HeartDarkenDuration => _uiData != null ? _uiData.ReviveHeartDarkenDuration : 0.25f;

        public void Initialize(
	        UiData uiData,
	        ICheckpointService checkpointService,
	        GameData gameData,
	        Chicken chicken,
	        IconsData iconsData
	    )
        {
            _uiData = uiData;
            _checkpointService = checkpointService;
            _gameData = gameData;
            _chicken = chicken;
            _iconsData = iconsData;
        }
        
        private void Awake()
        {
	        _withdrawText.text = WithdrawText;
	        _goText.text = GoText;
	        _reviveText.text = ReviveText;
	        _balanceValueText.text = BalanceValueText;
	        
	        _goNextButton.onClick.AddListener(OnGoPressed);
	        _reviveButton.onClick.AddListener(OnRevivePressed);
	        _withdrawButton.onClick.AddListener(() => OnWithdrawPress?.Invoke());

	        if (_reviveTutorArrow != null)
	        {
		        _arrowStartAnchoredPosition = _reviveTutorArrow.rectTransform.anchoredPosition;
		        _arrowStartCaptured = true;
		        SetArrowVisible(false);
	        }
        }

        private void OnGoPressed()
        {
	        OnNextPressed?.Invoke();
	        
	        _goNextButton.interactable = false;
	        
	        if (_checkpointService.GetCurrentCheckpoint != _gameData.LoseAfterCheckpoint)
	        {
		        StartCoroutine(WaitEndJump());
	        }
	        
	        return;

	        IEnumerator WaitEndJump()
	        {
		        yield return new WaitForSeconds(_gameData.TimeToStepMove);
		        
		        _goNextButton.interactable = true;
	        }
        }

        private void OnRevivePressed()
        {
	        OnRevivePress?.Invoke();
	        StopArrowAnimation();

	        if (_reviveTutorArrow != null)
	        {
		        var direction = ArrowFlyOffDirection == Vector2.zero ? Vector2.up : ArrowFlyOffDirection.normalized;
		        var startPosition = _arrowStartCaptured ? _arrowStartAnchoredPosition : _reviveTutorArrow.rectTransform.anchoredPosition;
		        var targetPosition = startPosition + direction * ArrowFlyOffDistance;

		        _arrowSequence?.Kill();
		        _arrowSequence = DOTween.Sequence();
		        _arrowSequence.Append(_reviveTutorArrow.rectTransform.DOAnchorPos(targetPosition, ArrowFlyOffDuration).SetEase(Ease.InBack));
		        _arrowSequence.Join(_reviveTutorArrow.rectTransform.DOScale(0.4f, ArrowFlyOffDuration).SetEase(Ease.InBack));
		        _arrowSequence.Join(_reviveTutorArrow.DOFade(0f, ArrowFlyOffDuration));
		        _arrowSequence.OnComplete(() =>
		        {
			        SetArrowVisible(false);
			        if (_arrowStartCaptured)
			        {
				        _reviveTutorArrow.rectTransform.anchoredPosition = _arrowStartAnchoredPosition;
			        }

			        ChangeChickenSprite();
		        });
	        }

	        if (_reviveHeartImage != null)
	        {
		        var color = _reviveHeartImage.color;
		        var targetColor = new Color(0.25f, 0.25f, 0.25f, color.a);
		        _reviveHeartImage.DOColor(targetColor, HeartDarkenDuration);
	        }
        }

        public void ShowReviveTutor()
        {
	        if (_reviveTutorArrow == null)
	        {
		        return;
	        }

	        if (_arrowStartCaptured == false)
	        {
		        _arrowStartAnchoredPosition = _reviveTutorArrow.rectTransform.anchoredPosition;
		        _arrowStartCaptured = true;
	        }

	        StopArrowAnimation();

	        _reviveTutorArrow.rectTransform.anchoredPosition = _arrowStartAnchoredPosition;
	        _reviveTutorArrow.rectTransform.localScale = Vector3.one;
	        SetArrowVisible(true, 0f);

	        _reviveTutorArrow.DOFade(1f, ArrowFadeDuration);

	        _arrowPulseTween = _reviveTutorArrow.rectTransform
		        .DOScale(ArrowPulseScale, ArrowPulseDuration)
		        .SetLoops(-1, LoopType.Yoyo)
		        .SetEase(Ease.InOutSine);
        }

        private void StopArrowAnimation()
        {
	        _arrowPulseTween?.Kill();
	        _arrowPulseTween = null;
	        _arrowSequence?.Kill();
	        _arrowSequence = null;
        }

        private void SetArrowVisible(bool isVisible, float alphaOverride = -1f)
        {
	        if (_reviveTutorArrow == null)
	        {
		        return;
	        }

	        _reviveTutorArrow.gameObject.SetActive(isVisible);
	        var color = _reviveTutorArrow.color;
	        color.a = alphaOverride >= 0f ? alphaOverride : isVisible ? color.a : 0f;
	        _reviveTutorArrow.color = color;
        }

        private void ChangeChickenSprite()
        {
	        var sequence = DOTween.Sequence();
	        sequence.Append(_chicken.transform.DOScale(Vector3.zero, _gameData.ReviveScaleTime / 2));
	        sequence.AppendCallback(() =>
	        {
		        _chicken.SetSprite(_iconsData.GetChickenSprite(true));
		        _chicken.OffsetPosition(false);
		        _chicken.GetAnimator.enabled = true;
	        });
	        sequence.Append(_chicken.transform.DOScale(Vector3.one, _gameData.ReviveScaleTime / 2));
	        sequence.OnComplete(() =>
	        {
		        _goNextButton.interactable = true;
	        });
        }
    }
}