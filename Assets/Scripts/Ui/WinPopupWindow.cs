using Move;
using TMPro;
using UnityEngine;

namespace Ui
{
	public class WinPopupWindow : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _rootCanvasGroup;
		[SerializeField] private Animator _notifyAnimator;
		[SerializeField] private Animator _winnerAnimator;
		
		[SerializeField] private TMP_Text _winnerText;
		
		[LunaPlaygroundField("Winner", 1, "Win Popup Window")]
		public string WinnerText;

		
		private IChickenMove _chickenMove;
		private GameHudWindow _gameHudWindow;

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
		
		private void OnWithdrawPressed()
		{
			
		}
		
		private void OnFinalMoveEnded()
		{
			
		}
	}
}

