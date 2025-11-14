using System.Collections;
using DG.Tweening;
using Move;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ui
{
	public class WinPopupWindow : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _winnerCanvasGroup;
		[SerializeField] private Animator _notifyAnimator;
		[SerializeField] private Animator _confettiAnimator;
		[SerializeField] private Animator _flashAnimator;
		[SerializeField] private RectTransform _windowContainer;
		[SerializeField] private Image _notifyImage;
		
		[SerializeField] private TMP_Text _winnerText;
		[SerializeField] private string _startTrigger = "Play";
		[SerializeField] private float _confettiAnimationDuration = 2f;
		[SerializeField] private float _windowSwingAngle = 15f;
		[SerializeField] private float _windowSwingDuration = 0.5f;
		
		[LunaPlaygroundField("Winner", 1, "Win Popup Window")]
		public string WinnerText;
		
		private IChickenMove _chickenMove;
		private GameHudWindow _gameHudWindow;
		private Coroutine _winSequenceCoroutine;

		public void Initialize(
			IChickenMove chickenMove, 
			GameHudWindow gameHudWindow
		)
		{
			_chickenMove = chickenMove;
			_gameHudWindow = gameHudWindow;

			_gameHudWindow.OnWithdrawPress += OnWithdrawPressed;
			_chickenMove.OnFinalMoveEnd += OnFinalMoveEnded;
		}

		private void Awake()
		{
			_winnerText.text = WinnerText;
		}

		private void Start()
		{
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
			
			PlayConfetti();
		}

		private void PlayConfetti()
		{
			if (_confettiAnimator == null || string.IsNullOrEmpty(_startTrigger))
			{
				return;
			}

			_confettiAnimator.ResetTrigger(_startTrigger);
			_confettiAnimator.SetTrigger(_startTrigger);
		}

		private void PlayFlash()
		{
			if (_flashAnimator == null || string.IsNullOrEmpty(_startTrigger))
			{
				return;
			}

			_flashAnimator.ResetTrigger(_startTrigger);
			_flashAnimator.SetTrigger(_startTrigger);
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
			
			var sequence = DOTween.Sequence();
			sequence.Append(_windowContainer.DOLocalRotate(new Vector3(0f, 0f, -_windowSwingAngle), _windowSwingDuration)
				.SetEase(Ease.InOutSine));
			sequence.Append(_windowContainer.DOLocalRotate(Vector3.zero, _windowSwingDuration)
				.SetEase(Ease.InOutSine));
			sequence.Append(_windowContainer.DOLocalRotate(new Vector3(0f, 0f, _windowSwingAngle), _windowSwingDuration)
				.SetEase(Ease.InOutSine));
			sequence.Append(_windowContainer.DOLocalRotate(Vector3.zero, _windowSwingDuration)
				.SetEase(Ease.InOutSine));
			sequence.SetLoops(-1, LoopType.Restart);
		}
		
		private void PlayNotification()
		{
			if (_notifyAnimator == null || string.IsNullOrEmpty(_startTrigger))
			{
				return;
			}

			_notifyImage.enabled = true;
			
			_notifyAnimator.ResetTrigger(_startTrigger);
			_notifyAnimator.SetTrigger(_startTrigger);
		}
	}
}

