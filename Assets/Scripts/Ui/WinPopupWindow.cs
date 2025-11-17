using Db.Sound;
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
		[SerializeField] private Animator _confettiAnimator;
		[SerializeField] private Animator _flashAnimator;
		[SerializeField] private RectTransform _windowContainer;
		[SerializeField] private Button _cashOutButton;
		[SerializeField] private TMP_Text _cashOutText;
		
		[SerializeField] private TMP_Text _winnerText;
		
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
		}
	}
}

