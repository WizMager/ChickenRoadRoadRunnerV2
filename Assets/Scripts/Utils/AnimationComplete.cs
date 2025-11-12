using System;
using UnityEngine;

namespace Utils
{
	public class AnimationComplete : StateMachineBehaviour
	{
		public Action OnComplete;
		
		private bool _isCompleted;

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!_isCompleted && stateInfo.normalizedTime >= 1)
			{
				_isCompleted = true;
				OnComplete?.Invoke();
			}
		}
	}
}

