using System.Collections.Generic;
using UnityEngine;

namespace Db
{
	[CreateAssetMenu(fileName = "SoundData", menuName = "Data/SoundData")]
	public class SoundData : ScriptableObject
	{
		[SerializeField] private AudioClip _chickenJump;
		[SerializeField] private AudioClip _carPassing;
		[SerializeField] private AudioClip _carStop;
		[SerializeField] private AudioClip _coinShuffle;
		[SerializeField] private AudioClip _winPopup;

		private Dictionary<SoundType, AudioClip> _soundMap;

		public AudioClip GetSound(SoundType soundType)
		{
			if (_soundMap == null)
			{
				_soundMap = new Dictionary<SoundType, AudioClip>
				{
					{ SoundType.ChickenJump, _chickenJump },
					{ SoundType.CarPassing, _carPassing },
					{ SoundType.CarStop, _carStop },
					{ SoundType.CoinShuffle, _coinShuffle },
					{ SoundType.WinPopup, _winPopup }
				};
			}

			return _soundMap.TryGetValue(soundType, out var clip) ? clip : null;
		}
	}
}

