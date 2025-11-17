using Db.Sound;
using DG.Tweening;
using Move;
using Services.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ui
{
	public class WinPopupWindow : MonoBehaviour
	{
		private static readonly int _play = Animator.StringToHash("Play");
		[SerializeField] private CanvasGroup _winnerCanvasGroup;
		[SerializeField] private Animator _notifyAnimator;
		[SerializeField] private Animator _confettiAnimator;
		[SerializeField] private Animator _flashAnimator;
		[SerializeField] private RectTransform _windowContainer;
		[SerializeField] private Image _notifyImage;
		[SerializeField] private RectTransform _notifyContainer;
		[SerializeField] private Button _cashOutButton;
		[SerializeField] private TMP_Text _cashOutText;
		
		[SerializeField] private TMP_Text _winnerText;
		[SerializeField] private float _windowSwingAngle = 15f;
		[SerializeField] private float _windowSwingDuration = 0.5f;
		
		[LunaPlaygroundField("Winner", 1, "Win Popup Window")]
		public string WinnerText;
		
		[LunaPlaygroundField("CashOut", 2, "Win Popup Window")]
		public string CashoutText;
		
		private IChickenMove _chickenMove;
		private GameHudWindow _gameHudWindow;
		private Coroutine _winSequenceCoroutine;
		private AudioService _audioService;

		public void Initialize(
			IChickenMove chickenMove, 
			GameHudWindow gameHudWindow,
			AudioService audioService
		)
		{
			_chickenMove = chickenMove;
			_gameHudWindow = gameHudWindow;
			_audioService = audioService;

			_gameHudWindow.OnWithdrawPress += OnWithdrawPressed;
			_chickenMove.OnFinalMoveEnd += OnFinalMoveEnded;
		}

		private void Awake()
		{
			_winnerText.text = WinnerText;
			_cashOutText.text = CashoutText;
			AdjustForAspectRatio();
		}

		private void Start()
		{
			_cashOutButton.onClick.AddListener(OnWithdrawPressed);
			_confettiAnimator.GetBehaviour<ConfettiAnimationState>().OnSignal += OnConfettiAnimationEnd;
		}

		private void OnConfettiAnimationEnd()
		{
			PlayFlash();
			ShowWindowContainer();
			PlayNotification();
		}
		
		private void OnWithdrawPressed()
		{
			Luna.Unity.Playable.InstallFullGame();
		}
		
		private void OnFinalMoveEnded()
		{
			if (!gameObject.activeInHierarchy)
			{
				return;
			}

			_winnerCanvasGroup.blocksRaycasts = true;
			
			PlayConfetti();
		}

		private void PlayConfetti()
		{
			if (_confettiAnimator == null)
			{
				return;
			}
			
			_audioService.PlayOneShotSound(ESoundType.Win);
			
			_confettiAnimator.SetTrigger(_play);
		}

		private void PlayFlash()
		{
			if (_flashAnimator == null)
			{
				return;
			}
			
			_flashAnimator.SetTrigger(_play);
		}

		private void ShowWindowContainer()
		{
			if (_winnerCanvasGroup != null)
			{
				_winnerCanvasGroup.alpha = 1f;
			}

			if (_windowContainer == null)
			{
				return;
			}

			_windowContainer.localRotation = Quaternion.identity;
			
			// var sequence = DOTween.Sequence();
			// sequence.Append(_windowContainer.DOLocalRotate(new Vector3(0f, 0f, -_windowSwingAngle), _windowSwingDuration)
			// 	.SetEase(Ease.InOutSine));
			// sequence.Append(_windowContainer.DOLocalRotate(Vector3.zero, _windowSwingDuration)
			// 	.SetEase(Ease.InOutSine));
			// sequence.Append(_windowContainer.DOLocalRotate(new Vector3(0f, 0f, _windowSwingAngle), _windowSwingDuration)
			// 	.SetEase(Ease.InOutSine));
			// sequence.Append(_windowContainer.DOLocalRotate(Vector3.zero, _windowSwingDuration)
			// 	.SetEase(Ease.InOutSine));
			// sequence.SetLoops(-1, LoopType.Restart);
		}
		
		private void PlayNotification()
		{
			if (_notifyAnimator == null)
			{
				return;
			}

			_audioService?.PlayOneShotSound(ESoundType.Notify);
			_notifyImage.enabled = true;
			
			_notifyAnimator.SetTrigger(_play);
		}
		
		private void AdjustForAspectRatio()
		{
			if (_notifyImage == null)
				return;

			if (Screen.width <= Screen.height) 
				return;

			var pos = _notifyContainer.anchoredPosition;
			pos.y = 0;
			_notifyContainer.anchoredPosition = pos;
		}
	}
}

