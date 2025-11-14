using System;
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
        [SerializeField] private Button _reviveButton;

        [SerializeField] private TMP_Text _withdrawText;
        [SerializeField] private TMP_Text _goText;
        [SerializeField] private TMP_Text _reviveText;
        
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

        private void Awake()
        {
	        _withdrawText.text = WithdrawText;
	        _goText.text = GoText;
	        _reviveText.text = ReviveText;
	        _balanceValueText.text = BalanceValueText;
	        
	        _goNextButton.onClick.AddListener(OnGoPressed);
        }

        private void OnGoPressed()
        {
	        OnNextPressed?.Invoke();
        }
    }
}