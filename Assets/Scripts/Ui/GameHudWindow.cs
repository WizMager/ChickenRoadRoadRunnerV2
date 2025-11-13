using System;
using System.Threading.Tasks;
using Car;
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
        [SerializeField] private Button _withdrawButton;
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

		[SerializeField] private TMP_Text _withdrawText;
		[SerializeField] private TMP_Text _goText;
		
		[LunaPlaygroundField("Withdraw", 1, "Game HUD Window")]
		public string WithdrawText;
		
		[LunaPlaygroundField("Go", 2, "Game HUD Window")]
		public string GoText;

        private GameData _gameData;
        private bool _isBlockPressButton;
        private int _checkpointIndex;
        private int _maxCheckpointIndex;
        private Tween _withdrawTween;
        private int _currentBalance;
        private ICarController _carController;

        public int CurrentScore => _currentBalance;
        
        public void Initialize(GameData gameData, int maxCheckpoint, ICarController carController)
        {
            _gameData = gameData;
            _maxCheckpointIndex = maxCheckpoint;
            _carController = carController;
            
            _goNextButton.onClick.AddListener(OnGoNextPress);
            _withdrawButton.onClick.AddListener(OnWithdrawPress);
            var color = new Color(1, 1, 1, 0);
            _greenIndicator.DOColor(color, 0.5f).SetLoops(-1, LoopType.Yoyo);

			AdjustForAspectRatio();
			
			ApplyInitial();
        }

        private void Awake()
        {
	        _withdrawText.text = WithdrawText;
	        _goText.text = GoText;
        }

        private void OnWithdrawPress()
        {
	        _goNextButton.interactable = false;
	        _withdrawButton.interactable = false;
	        
	        Luna.Unity.Playable.InstallFullGame();
        }

        private void OnGoNextPress()
        {
            if (_isBlockPressButton || _checkpointIndex >= _maxCheckpointIndex)
                return;
            
            _isBlockPressButton = true;
            _checkpointIndex++;
            
			ApplyStepIfConfiguredWinners(_checkpointIndex);
			
			if (_carController.IsSaveJump)
			{
				ChangeWithdrawSumText(_checkpointIndex);
			}
			
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
			var prevValue = _currentBalance;
			_currentBalance += step;
			
			_withdrawTween = DOVirtual.Int(prevValue, _currentBalance, 0.5f, value =>
			{
				_balanceText.text = $"{value}.00";
				_withdrawSumText.text = $"{value}.00";
			});
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