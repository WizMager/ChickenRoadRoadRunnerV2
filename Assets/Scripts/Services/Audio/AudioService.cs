using Db.Sound;
using UnityEngine;

namespace Services.Audio
{
	public class AudioService : MonoBehaviour
	{
		[SerializeField] private AudioSource _oneShotAudioSource;
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private AudioSource _backgroundMusicSource;
		
		private SoundData _soundData;

		public void Initialize(SoundData soundData)
		{
			_soundData = soundData;
		}
		
		private void Start()
		{
			if (_backgroundMusicSource == null || _soundData == null)
				return;
			
			_backgroundMusicSource.clip = _soundData.GetSound(ESoundType.Music);
			_backgroundMusicSource.loop = true;
			_backgroundMusicSource.Play();
		}

		public void PlayOneShotSound(ESoundType soundType)
		{
			if (_soundData == null || _oneShotAudioSource == null)
				return;

			var clip = _soundData.GetSound(soundType);
			
			if (clip != null)
			{
				_oneShotAudioSource.PlayOneShot(clip);
			}
		}

		public void PlaySound(ESoundType soundType, bool isLoop = false)
		{
			if (_soundData == null || _audioSource == null)
				return;

			var clip = _soundData.GetSound(soundType);
			
			if (clip != null)
			{
				_audioSource.clip = clip;
				_audioSource.loop = isLoop;
				_audioSource.Play();
			}
		}
		
		public void StopSound()
		{
			if (_audioSource == null || !_audioSource.isPlaying)
				return;

			_audioSource.Stop();
		}
	}
}



