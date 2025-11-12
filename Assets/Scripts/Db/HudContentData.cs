using System.Collections.Generic;
using UnityEngine;

namespace Db
{
	[CreateAssetMenu(fileName = "HudContentData", menuName = "Data/HudContentData")]
	public class HudContentData : ScriptableObject
	{
		[SerializeField] private List<PressStep> _steps;
		[SerializeField] private List<int> _valueFromSteps;

		public HudEntry Initial => _steps[0].Values;
		
		public HudEntry GetStepForPress(int pressIndex)
		{
			foreach (var step in _steps)
			{
				if (step.PressIndex == pressIndex)
				{
					return step.Values;
				}
			}

			return new HudEntry();
		}
		
		public int GetValueFromStep(int pressIndex)
		{
			return _valueFromSteps[pressIndex];
		}
	}
}

