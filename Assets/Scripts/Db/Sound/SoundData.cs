using System.Collections.Generic;
using UnityEngine;

namespace Db.Sound
{
	[CreateAssetMenu(fileName = "SoundData", menuName = "Data/SoundData")]
	public class SoundData : ScriptableObject
	{
		[SerializeField] private List<SoundVo> _sounds;

		public AudioClip GetSound(ESoundType soundType)
		{
			foreach (var soundVo in _sounds)
			{
				if (soundVo.Type != soundType)
					continue;

				return soundVo.Clip;
			}
			
			return null;
		}
	}
}



