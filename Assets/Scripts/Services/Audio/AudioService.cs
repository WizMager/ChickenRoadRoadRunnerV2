using Db.Sound;
using UnityEngine;

namespace Services.Audio
{
	public class AudioService : MonoBehaviour
	{
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private AudioSource _backgroundMusicSource;
		[SerializeField] private SoundData _soundData;

		private void Start()
		{
			if (_backgroundMusicSource == null || _soundData == null)
				return;
			
			//_backgroundMusicSource.clip = _soundData.GetSound(ESoundType.Music);
			_backgroundMusicSource.Play();
		}

		public void PlaySound(ESoundType soundType)
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



