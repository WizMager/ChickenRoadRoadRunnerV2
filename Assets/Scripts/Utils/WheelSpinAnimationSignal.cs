using System;
using UnityEngine;

namespace Utils
{
	public class WheelSpinAnimationSignal : StateMachineBehaviour
	{
		public Action OnSignal;

		[Range(0f, 1f)]
		[SerializeField] private float _threshold = 0.9f;

		private bool _isRaised;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_isRaised = false;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_isRaised || stateInfo.normalizedTime < _threshold)
				return;

			_isRaised = true;
			OnSignal?.Invoke();
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_isRaised = false;
		}
	}
}




