using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
	public class FinalPopupWindow : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _rootCanvasGroup;
		[SerializeField] private Image _backdrop; // затемнение
		[SerializeField] private RectTransform _coinTop;
		[SerializeField] private RectTransform _coinLeft;
		[SerializeField] private RectTransform _coinRight;
		[SerializeField] private float _fadeDuration = 0.25f;
		[SerializeField] private float _waitBeforeShuffle = 2f;
		[SerializeField] private float _swapDuration = 0.5f;
		[SerializeField] private float _swapArcHeight = 60f;
		[SerializeField] private int _swapIterations = 5;
		[SerializeField] private float _convergeDuration = 0.5f;
		[SerializeField] private float _zoomScale = 1.6f;
		[SerializeField] private float _zoomDuration = 0.45f;
		[SerializeField] private float _flipDuration = 0.5f;
		[SerializeField] private float _autoHideDelay = 2f;

		private Sequence _sequence;
		private Vector3 _startTop;
		private Vector3 _startLeft;
		private Vector3 _startRight;

		private void Awake()
		{
			_rootCanvasGroup.alpha = 0f;
			gameObject.SetActive(false);
		}

		public void PlaySequence()
		{
			_sequence?.Kill();
			gameObject.SetActive(true);
			_rootCanvasGroup.alpha = 0f;

			_startTop = _coinTop.anchoredPosition;
			_startLeft = _coinLeft.anchoredPosition;
			_startRight = _coinRight.anchoredPosition;

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

			_sequence.AppendInterval(_waitBeforeShuffle);

			var pattern = new[]
			{
				new[] { _coinTop, _coinLeft },
				new[] { _coinLeft, _coinRight },
				new[] { _coinRight, _coinTop }
			};

			for (int i = 0; i < _swapIterations; i++)
			{
				var currentPair = pattern[i % pattern.Length];
				var first = currentPair[0];
				var second = currentPair[1];

				_sequence.AppendCallback(() => StartArcSwap(first, second));
				_sequence.AppendInterval(_swapDuration);
			}

			// Схождение к центру
			_sequence.Append(_coinTop.DOAnchorPos(center, _convergeDuration));
			_sequence.Join(_coinLeft.DOAnchorPos(center, _convergeDuration));
			_sequence.Join(_coinRight.DOAnchorPos(center, _convergeDuration));

			// Зум верхней монетки, выключаем нижние
			_sequence.AppendCallback(() =>
			{
				_coinLeft.gameObject.SetActive(false);
				_coinRight.gameObject.SetActive(false);
			});
			_sequence.Append(_coinTop.DOScale(_zoomScale, _zoomDuration).SetEase(Ease.OutBack));

			// Флип верхней монетки
			_sequence.Append(_coinTop.DOLocalRotate(new Vector3(0f, 180f, 0f), _flipDuration, RotateMode.FastBeyond360));

			// Авто-сокрытие
			_sequence.AppendInterval(_autoHideDelay);
			_sequence.Append(_rootCanvasGroup.DOFade(0f, _fadeDuration));
			if (_backdrop != null)
			{
				_sequence.Join(_backdrop.DOFade(0f, _fadeDuration));
			}
			_sequence.OnComplete(HideAndReset);
		}

		private void HideAndReset()
		{
			_coinTop.localScale = Vector3.one;
			_coinTop.localRotation = Quaternion.identity;
			_coinTop.anchoredPosition = _startTop;
			_coinLeft.anchoredPosition = _startLeft;
			_coinRight.anchoredPosition = _startRight;

			gameObject.SetActive(false);
		}

		private void StartArcSwap(RectTransform first, RectTransform second)
		{
			var startFirst = first.anchoredPosition;
			var startSecond = second.anchoredPosition;
			var halfDuration = _swapDuration * 0.5f;
			var offset = new Vector2(0f, _swapArcHeight);

			var seq = DOTween.Sequence();
			seq.Join(first.DOAnchorPos(startFirst + offset, halfDuration).SetEase(Ease.OutQuad));
			seq.Join(second.DOAnchorPos(startSecond - offset, halfDuration).SetEase(Ease.OutQuad));
			seq.Append(first.DOAnchorPos(startSecond, halfDuration).SetEase(Ease.InOutQuad));
			seq.Join(second.DOAnchorPos(startFirst, halfDuration).SetEase(Ease.InOutQuad));
		}

		private void OnDisable()
		{
			_sequence?.Kill();
		}
	}
}

