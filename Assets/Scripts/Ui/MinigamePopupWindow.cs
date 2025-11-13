using System;
using System.Collections.Generic;
using Db;
using Db.Sound;
using DG.Tweening;
using Services.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Ui
{
	public class MinigamePopupWindow : MonoBehaviour
	{
		public Action OnCompleteMiniGame;
		
		[SerializeField] private CanvasGroup _rootCanvasGroup;
		[SerializeField] private Image _backdrop;
		[SerializeField] private RectTransform _coinTop;
		[SerializeField] private RectTransform _coinLeft;
		[SerializeField] private RectTransform _coinRight;
		[SerializeField] private TMP_Text _coinTopText;
		[SerializeField] private TMP_Text _coinLeftText;
		[SerializeField] private TMP_Text _coinRightText;
		[SerializeField] private Animator _coinTopAnimator;
		[SerializeField] private Animator _coinLeftAnimator;
		[SerializeField] private Animator _coinRightAnimator;
		[SerializeField] private Button _coinTopButton;
		[SerializeField] private Button _coinLeftButton;
		[SerializeField] private Button _coinRightButton;
		[SerializeField] private float _fadeDuration = 0.25f;
		[SerializeField] private float _waitBeforeShuffle = 2f;
		[SerializeField] private float _swapDuration = 0.35f;
		[SerializeField] private int _swapIterations = 5;
		[SerializeField] private float _convergeDuration = 0.5f;
		[SerializeField] private float _zoomScale = 1.6f;
		[SerializeField] private float _zoomDuration = 0.45f;
		[SerializeField] private float _flipDuration = 0.5f;
		[SerializeField] private float _autoHideDelay = 2f;
		[SerializeField] private Vector2 _flyAwayDirection = new Vector2(0f, 800f);
		[SerializeField] private float _flyAwayDuration = 0.45f;

		[LunaPlaygroundField("Cashback", 1, "Minigame Popup Window")]
		public string CashbackText;
		
		[LunaPlaygroundField("Grand Jackpot", 2, "Minigame Popup Window")]
		public string GrandJackpotText;
		
	private Sequence _sequence;
	private Vector3 _startTop;
	private Vector3 _startLeft;
	private Vector3 _startRight;
	private bool _isFirstRotate = true;
	private AudioService _audioService;
	private Vector2 _centerPosition;
	private bool _isAwaitingSelection;
	private readonly List<Button> _coinButtons = new List<Button>();
	private RectTransform[] _coinRects;
	private TMP_Text[] _coinTexts;
	private Vector2[] _initialPositions;
	private int[] _coinSlotsByCoinIndex;
	private static readonly int[][] SwapPairs = new int[][]
	{
		new int[] { 0, 1 },
		new int[] { 1, 2 },
		new int[] { 2, 0 }
	};

	public void Initialize(AudioService audioService)
	{
		_audioService = audioService;
	}

	private void Awake()
	{
		_rootCanvasGroup.alpha = 0f;
		gameObject.SetActive(false);
		EnsureCoinCaches();
		RegisterButtons();
		SetButtonsInteractable(false);
	}

	private void OnEnable()
	{
		var halfRotate = _coinTopAnimator.GetBehaviour<HalfRotate>();
		if (halfRotate != null)
		{
			halfRotate.OnRotate += ChangeText;
		}
	}

	private void OnDisable()
	{
		var halfRotate = _coinTopAnimator.GetBehaviour<HalfRotate>();
		if (halfRotate != null)
		{
			halfRotate.OnRotate -= ChangeText;
		}
		_sequence?.Kill();
		_isAwaitingSelection = false;
	}

		public void PlaySequence()
		{
			_sequence?.Kill();
			gameObject.SetActive(true);
			_rootCanvasGroup.alpha = 0f;

			EnsureCoinCaches();
			_isFirstRotate = true;
			_isAwaitingSelection = false;
			SetButtonsInteractable(false);

			_startTop = _coinTop.anchoredPosition;
			_startLeft = _coinLeft.anchoredPosition;
			_startRight = _coinRight.anchoredPosition;
			_initialPositions[0] = _startTop;
			_initialPositions[1] = _startLeft;
			_initialPositions[2] = _startRight;
			for (var index = 0; index < _coinSlotsByCoinIndex.Length; index++)
			{
				_coinSlotsByCoinIndex[index] = index;
			}
			_coinTopText.text = "x10";
			_coinLeftText.text = GrandJackpotText;
			_coinRightText.text = CashbackText;

			_coinTop.gameObject.SetActive(true);
			_coinLeft.gameObject.SetActive(true);
			_coinRight.gameObject.SetActive(true);

			var center = ( _startTop + _startLeft + _startRight ) / 3f;

			_sequence = DOTween.Sequence();
			_sequence.Append(_rootCanvasGroup.DOFade(1f, _fadeDuration));
			if (_backdrop != null)
			{
				var col = _backdrop.color;
				col.a = 0f;
				_backdrop.color = col;
				_sequence.Join(_backdrop.DOFade(0.6f, _fadeDuration));
			}

			_coinTopAnimator.SetTrigger("Rotate");
			_coinLeftAnimator.SetTrigger("Rotate");
			_coinRightAnimator.SetTrigger("Rotate");
			
			_sequence.AppendInterval(_waitBeforeShuffle);
			
			for (var i = 0; i < _swapIterations; i++)
			{
				var pairIndex = Random.Range(0, SwapPairs.Length);
				var first = SwapPairs[pairIndex][0];
				var second = SwapPairs[pairIndex][1];
				var firstCoin = _coinRects[first];
				var secondCoin = _coinRects[second];

				_sequence.Append(Swap(firstCoin, secondCoin, _swapDuration));
				SwapSlots(first, second);
				_sequence.AppendCallback(() => _audioService?.PlaySound(ESoundType.CoinShuffle));
			}
			
			_sequence.AppendCallback(SnapCoinsToSlots);
			_sequence.AppendCallback(() =>
			{
				_centerPosition = center;
				PrepareForSelection();
			});
		}

		private void PrepareForSelection()
		{
			_isAwaitingSelection = true;
			SetButtonsInteractable(true);
			_coinTop.SetAsLastSibling();
		}

		private void HandleCoinSelection(RectTransform selectedCoin, TMP_Text selectedText)
		{
			if (!_isAwaitingSelection)
				return;

			EnsureCoinCaches();
			_isAwaitingSelection = false;
			SetButtonsInteractable(false);

			_sequence?.Kill();

			selectedCoin.SetAsLastSibling();
			selectedCoin.localRotation = Quaternion.identity;
			selectedCoin.localScale = Vector3.one;
			selectedText.gameObject.SetActive(true);

			var resultSequence = DOTween.Sequence();
			var hasAppended = false;
			for (var i = 0; i < _coinRects.Length; i++)
			{
				var coin = _coinRects[i];
				if (coin == selectedCoin)
					continue;

				var tween = coin.DOAnchorPos(coin.anchoredPosition + _flyAwayDirection, _flyAwayDuration).SetEase(Ease.InQuad);
				if (!hasAppended)
				{
					resultSequence.Append(tween);
					hasAppended = true;
				}
				else
				{
					resultSequence.Join(tween);
				}
				var index = i;
				tween.OnComplete(() =>
				{
					_coinRects[index].gameObject.SetActive(false);
					_coinTexts[index].gameObject.SetActive(false);
				});
			}

			if (!hasAppended)
			{
				resultSequence.Append(selectedCoin.DOAnchorPos(_centerPosition, _convergeDuration).SetEase(Ease.OutQuad));
			}
			else
			{
				resultSequence.Append(selectedCoin.DOAnchorPos(_centerPosition, _convergeDuration).SetEase(Ease.OutQuad));
			}

			resultSequence.Append(selectedCoin.DOScale(_zoomScale, _zoomDuration).SetEase(Ease.OutBack));
			var halfFlipDuration = _flipDuration * 0.5f;
			resultSequence.Append(selectedCoin.DOLocalRotate(new Vector3(0f, 90f, 0f), halfFlipDuration - 0.05f, RotateMode.Fast));
			resultSequence.AppendCallback(() =>
			{
				selectedText.text = "x10";
			});
			resultSequence.Append(selectedCoin.DOLocalRotate(new Vector3(0f, 0f, 0f), halfFlipDuration + 0.05f, RotateMode.Fast));
			resultSequence.AppendInterval(_autoHideDelay);
			resultSequence.Append(_rootCanvasGroup.DOFade(0f, _fadeDuration));
			if (_backdrop != null)
			{
				resultSequence.Join(_backdrop.DOFade(0f, _fadeDuration));
			}

			resultSequence.OnComplete(HideAndReset);
			_sequence = resultSequence;
		}

		private void RegisterButtons()
		{
			EnsureCoinCaches();
			_coinButtons.Clear();

			if (_coinTopButton != null)
			{
				_coinButtons.Add(_coinTopButton);
				_coinTopButton.onClick.AddListener(OnTopCoinClicked);
			}
			if (_coinLeftButton != null)
			{
				_coinButtons.Add(_coinLeftButton);
				_coinLeftButton.onClick.AddListener(OnLeftCoinClicked);
			}
			if (_coinRightButton != null)
			{
				_coinButtons.Add(_coinRightButton);
				_coinRightButton.onClick.AddListener(OnRightCoinClicked);
			}
		}

		private void OnTopCoinClicked() => HandleCoinSelection(_coinTop, _coinTopText);
		private void OnLeftCoinClicked() => HandleCoinSelection(_coinLeft, _coinLeftText);
		private void OnRightCoinClicked() => HandleCoinSelection(_coinRight, _coinRightText);

		private void SetButtonsInteractable(bool state)
		{
			foreach (var button in _coinButtons)
			{
				button.interactable = state;
			}
		}

		private void HideAndReset()
		{
			EnsureCoinCaches();
			_coinTop.localScale = Vector3.one;
			_coinTop.localRotation = Quaternion.identity;
			_coinTop.anchoredPosition = _startTop;
			_coinLeft.anchoredPosition = _startLeft;
			_coinRight.anchoredPosition = _startRight;
			_initialPositions[0] = _startTop;
			_initialPositions[1] = _startLeft;
			_initialPositions[2] = _startRight;
			for (var i = 0; i < _coinSlotsByCoinIndex.Length; i++)
			{
				_coinSlotsByCoinIndex[i] = i;
			}

			_coinLeft.gameObject.SetActive(true);
			_coinRight.gameObject.SetActive(true);
			_coinTop.gameObject.SetActive(true);
			_coinLeftText.gameObject.SetActive(true);
			_coinRightText.gameObject.SetActive(true);
			_coinTopText.gameObject.SetActive(true);
			_coinTopText.text = "x10";
			_coinLeftText.text = "Grand Jackpot";
			_coinRightText.text = "Cashback 30%";
			_rootCanvasGroup.alpha = 0f;
			SetButtonsInteractable(false);

			gameObject.SetActive(false);
			
			OnCompleteMiniGame?.Invoke();
		}

		private static Sequence Swap(RectTransform a, RectTransform b, float duration)
		{
			var seq = DOTween.Sequence();
			var posA = a.anchoredPosition;
			var posB = b.anchoredPosition;
			seq.Join(a.DOAnchorPos(posB, duration).SetEase(Ease.InOutQuad));
			seq.Join(b.DOAnchorPos(posA, duration).SetEase(Ease.InOutQuad));
			
			return seq;
		}

		private void SwapSlots(int firstCoinIndex, int secondCoinIndex)
		{
			var temp = _coinSlotsByCoinIndex[firstCoinIndex];
			_coinSlotsByCoinIndex[firstCoinIndex] = _coinSlotsByCoinIndex[secondCoinIndex];
			_coinSlotsByCoinIndex[secondCoinIndex] = temp;
		}

		private void SnapCoinsToSlots()
		{
			for (var i = 0; i < _coinRects.Length; i++)
			{
				var slotIndex = _coinSlotsByCoinIndex[i];
				_coinRects[i].anchoredPosition = _initialPositions[slotIndex];
				_coinRects[i].localRotation = Quaternion.identity;
				_coinRects[i].gameObject.SetActive(true);
				_coinTexts[i].gameObject.SetActive(true);
			}
		}

		private void EnsureCoinCaches()
		{
			if (_coinRects != null)
				return;

			_coinRects = new[] { _coinTop, _coinLeft, _coinRight };
			_coinTexts = new[] { _coinTopText, _coinLeftText, _coinRightText };
			_initialPositions = new Vector2[_coinRects.Length];
			_coinSlotsByCoinIndex = new int[_coinRects.Length];
		}

		private void ChangeText()
		{
			if (_isFirstRotate)
			{
				_isFirstRotate = false;
				_coinTopText.text = "?";
				_coinLeftText.text = "?";
				_coinRightText.text = "?";
			}
			else
			{
				_coinTopText.text = "x10";
			}
		}

		private void OnDestroy()
		{
			if (_coinTopButton != null)
			{
				_coinTopButton.onClick.RemoveListener(OnTopCoinClicked);
			}
			if (_coinLeftButton != null)
			{
				_coinLeftButton.onClick.RemoveListener(OnLeftCoinClicked);
			}
			if (_coinRightButton != null)
			{
				_coinRightButton.onClick.RemoveListener(OnRightCoinClicked);
			}
		}
	}
}

