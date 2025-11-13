using Db;
using Db.Sound;
using DG.Tweening;
using Services.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using ESoundType = Db.Sound.ESoundType;

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
	}
}

