using System;
using System.Threading.Tasks;
using Db;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Ui
{
    public class GameHudWindow : MonoBehaviour
    {
        public Action OnNextPressed;
        
        [SerializeField] private Button _goNextButton;
        [SerializeField] private Image _greenIndicator;
        //Winline container
		[SerializeField] private Image _profileIcon;
		[SerializeField] private TextMeshProUGUI _nicknameText;
		[SerializeField] private TextMeshProUGUI _winSumText;
		[SerializeField] private HudContentData _hudData;
		//Withdraw container
		[SerializeField] private TextMeshProUGUI _withdrawSumText;
		//Balance
		[SerializeField] private TextMeshProUGUI _balanceText;
		[SerializeField] private RectTransform _rootContainer;

        private GameData _gameData;
        private bool _isBlockPressButton;
        private int _checkpointIndex;
        private int _maxCheckpointIndex;
        private Tween _withdrawTween;
        private int _currentBalance;
        
        public void Initialize(GameData gameData, int maxCheckpoint)
        {
            _gameData = gameData;
            _maxCheckpointIndex = maxCheckpoint;
            _goNextButton.onClick.AddListener(OnGoNextPress);
            var color = new Color(1, 1, 1, 0);
            _greenIndicator.DOColor(color, 0.5f).SetLoops(-1, LoopType.Yoyo);

			AdjustForAspectRatio();
			
			ApplyInitial();
        }
        
        private void OnGoNextPress()
        {
            if (_isBlockPressButton || _checkpointIndex >= _maxCheckpointIndex)
                return;

            _isBlockPressButton = true;
            _checkpointIndex++;
            
			ApplyStepIfConfiguredWinners(_checkpointIndex);
			ChangeWithdrawSumText(_checkpointIndex);
			
            OnNextPressed?.Invoke();
            UnblockButton();
        }

        private async void UnblockButton()
        {
            await Task.Delay(TimeSpan.FromSeconds(_gameData.TimeToStepMove));

            _isBlockPressButton = false;
        }
        
        public void Reset()
        {
            _checkpointIndex = 0;
            _isBlockPressButton = false;
            _currentBalance = 0;
			ApplyInitial();
			ChangeWithdrawSumText(0);
        }

		private void ApplyInitial()
		{
			if (_hudData == null) 
				return;
			
			_profileIcon.color = _hudData.Initial.IconColor;
			_nicknameText.text = _hudData.Initial.Nickname;
			_winSumText.text = _hudData.Initial.AmountText;
		}

		private void ApplyStepIfConfiguredWinners(int pressIndex)
		{
			if (_hudData == null) 
				return;
			
			var step = _hudData.GetStepForPress(pressIndex);
			
			if (step.Nickname == null) 
				return;
			
			_profileIcon.color = step.IconColor;
			_nicknameText.text = step.Nickname;
			_winSumText.text = step.AmountText;
		}
		
		private void ChangeWithdrawSumText(int pressIndex)
		{
			if (_hudData == null) 
				return;
			
			_withdrawTween?.Kill();
			
			var step = _hudData.GetValueFromStep(pressIndex);
			_withdrawTween = DOVirtual.Int(_currentBalance, step, 0.5f, value =>
			{
				_balanceText.text = $"{value}.00";
				_withdrawSumText.text = $"{value}.00";
			});
			_currentBalance += step;
		}

		private void AdjustForAspectRatio()
		{
			if (_rootContainer == null)
				return;

			if (Screen.width <= Screen.height) 
				return;
			
			var scale = _rootContainer.localScale;
			scale.y = 0.7f;
			_rootContainer.localScale = scale;
		}
    }
}