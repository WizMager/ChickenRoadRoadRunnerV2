using System;
using UnityEngine;

namespace Utils
{
	public class WheelSpinAnimationSignal : StateMachineBehaviour
	{
		public Action OnSignal;
		public Action OnSpinEnd;

		[Range(0f, 1f)]
		[SerializeField] private float _threshold = 0.9f;

		[Range(0f, 1f)]
		[SerializeField] private float _spinEndThreshold = 0.6f;

		private bool _isRaised;
		private bool _isSpinEnd;
		

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!_isRaised && stateInfo.normalizedTime >= _threshold)
			{
				_isRaised = true;
				OnSignal?.Invoke();
			}

			if (!_isSpinEnd && stateInfo.normalizedTime >= _spinEndThreshold)
			{
				_isSpinEnd = true;
				OnSpinEnd?.Invoke();
			}
		}
	}
}




