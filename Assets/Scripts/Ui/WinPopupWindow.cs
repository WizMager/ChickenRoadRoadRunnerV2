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
	public class WinPopupWindow : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _rootCanvasGroup;
		[SerializeField] private Image _backdrop;
		[SerializeField] private Image _frameImage;
		[SerializeField] private TMP_Text _valueText;
		[SerializeField] private Animator _coinsAnimator;
		[SerializeField] private Animator _frameAnimator;
		[SerializeField] private RectTransform _notificationPanel;
		[SerializeField] private Button _getBonus;
		[SerializeField] private float _fadeDuration = 0.25f;
		[SerializeField] private float _swingAngle = 30f;
		[SerializeField] private float _swingDuration = 1f;
		[SerializeField] private float _textChangeDuration = 2f;
		[SerializeField] private float _notificationSlideDuration = 0.5f;
		[SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
		[SerializeField] private RectTransform _frameContainer;

		[SerializeField] private TMP_Text _notifyNew;
		[SerializeField] private TMP_Text _notifyNewTransaction;
		[SerializeField] private TMP_Text _notifyTransaction;
		[SerializeField] private TMP_Text _notifyBalance;
		
		[SerializeField] private TMP_Text _congratText;
		
		[SerializeField] private TMP_Text _getBonusText;
		
		[LunaPlaygroundField("Notify now", 1, "Win Popup Window")]
		public string NotifyNowText;
		
		[LunaPlaygroundField("New transaction", 2, "Win Popup Window")]
		public string NewTransactionText;
		
		[LunaPlaygroundField("Transaction", 3, "Win Popup Window")]
		public string TransactionText;
		
		[LunaPlaygroundField("Balance", 4, "Win Popup Window")]
		public string BalanceText;
		
		[LunaPlaygroundField("Congrat", 5, "Win Popup Window")]
		public string CongratText;
		
		[LunaPlaygroundField("Get Bonus", 6, "Win Popup Window")]
		public string GetBonusText;
		
	private Sequence _sequence;
	private int _startValue = 3150;
	private int _endValue = 31500;
	private Vector2 _notificationStartPosition;
	private Vector2 _notificationEndPosition;
	private AudioService _audioService;

	public void Initialize(AudioService audioService)
	{
		_audioService = audioService;
	}

	private void Awake()
	{
		_rootCanvasGroup.alpha = 0f;
		gameObject.SetActive(false);
		
		_getBonus.onClick.AddListener(OnClickGetBonus);
		AdjustForAspectRatio();
		
		_notifyNew.text = NotifyNowText;
		_notifyNewTransaction.text = NewTransactionText;
		_notifyTransaction.text = TransactionText;
		_notifyBalance.text = BalanceText;
		_congratText.text = CongratText;
		_getBonusText.text = GetBonusText;
	}

	private void OnClickGetBonus()
	{
		Luna.Unity.Playable.InstallFullGame();
	}

	private void OnEnable()
	{
		var animationComplete = _frameAnimator.GetBehaviour<AnimationComplete>();
		if (animationComplete != null)
		{
			animationComplete.OnComplete += OnCoinsAnimationComplete;
		}

		if (_notificationPanel != null)
		{
			_notificationEndPosition = _notificationPanel.anchoredPosition;
			var canvas = GetComponentInParent<Canvas>();
			var canvasRect = canvas != null ? canvas.GetComponent<RectTransform>() : null;
			var offsetY = canvasRect != null ? canvasRect.rect.height : 1000f;
			_notificationStartPosition = new Vector2(_notificationEndPosition.x, _notificationEndPosition.y + offsetY);
			_notificationPanel.anchoredPosition = _notificationStartPosition;
			_notificationPanel.gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		var animationComplete = _frameAnimator.GetBehaviour<AnimationComplete>();
		if (animationComplete != null)
		{
			animationComplete.OnComplete -= OnCoinsAnimationComplete;
		}
	}

	private void OnCoinsAnimationComplete()
	{
		ShowNotification();
	}

	private void ShowNotification()
	{
		if (_notificationPanel == null)
			return;

		_notificationPanel.gameObject.SetActive(true);
		_notificationPanel.anchoredPosition = _notificationStartPosition;
		_notificationPanel.DOAnchorPos(_notificationEndPosition, _notificationSlideDuration).SetEase(Ease.OutBack);
	}

	public void Show(bool isLose = false, int score = 0)
	{
		_sequence?.Kill();
		gameObject.SetActive(true);
		_rootCanvasGroup.alpha = 0f;
		_valueText.text = $"{_startValue}€";
		_frameImage.transform.localRotation = Quaternion.identity;

		_sequence = DOTween.Sequence();
		
		_coinsAnimator.SetTrigger("Play");
		_audioService?.PlaySound(ESoundType.WinPopup);
		_sequence.Append(_rootCanvasGroup.DOFade(1f, _fadeDuration));
		if (_backdrop != null)
		{
			var col = _backdrop.color;
			col.a = 0f;
			_backdrop.color = col;
			_sequence.Join(_backdrop.DOFade(0.6f, _fadeDuration));
		}

		if (isLose)
		{
			_notifyTransaction.text += $"{score} EUR";
			_notifyBalance.text += $"{score} EUR";
			
			_sequence.Append(DOVirtual.Int(0, score, _textChangeDuration, value =>
			{
				_valueText.text = value.ToString();
			}));
		}
		else
		{
			_notifyTransaction.text += "31500 EUR";
			_notifyBalance.text += "31500 EUR";
			
			_sequence.Append(DOVirtual.Int(_startValue, _endValue, _textChangeDuration, value =>
			{
				_valueText.text = $"{value}€";
			}));
		}
		
		_sequence.AppendCallback(() =>
		{
			var swingSequence = DOTween.Sequence();
			swingSequence.Append(_frameImage.transform.DOLocalRotate(new Vector3(0f, 0f, -_swingAngle), _swingDuration / 2f).SetEase(Ease.InOutSine));
			swingSequence.Append(_frameImage.transform.DOLocalRotate(new Vector3(0f, 0f, _swingAngle), _swingDuration).SetEase(Ease.InOutSine));
			swingSequence.Append(_frameImage.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), _swingDuration / 2f).SetEase(Ease.InOutSine));
			swingSequence.SetLoops(-1, LoopType.Restart);
		});
	}
		
		private void AdjustForAspectRatio()
		{
			if (_verticalLayoutGroup == null)
				return;

			if (Screen.width <= Screen.height) 
				return;

			_frameContainer.localScale = new Vector3(0.5f, 0.5f, 1);
			var anchoredPosition = _frameContainer.anchoredPosition;
			anchoredPosition.y -= 50f;
			_frameContainer.anchoredPosition = anchoredPosition;
		}
	}
}

