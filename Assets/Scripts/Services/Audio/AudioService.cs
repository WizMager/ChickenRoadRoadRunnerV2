using Db;
using UnityEngine;

namespace Services.Audio
{
	public class AudioService : MonoBehaviour
	{
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private SoundData _soundData;

		public void PlaySound(SoundType soundType)
		{
			if (_soundData == null || _audioSource == null)
				return;

			var clip = _soundData.GetSound(soundType);
			if (clip != null)
			{
				_audioSource.PlayOneShot(clip);
			}
		}
	}
}

