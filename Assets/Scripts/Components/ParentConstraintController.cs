using UnityEngine;

namespace Components
{
	public class ParentConstraintController : MonoBehaviour
	{
		[SerializeField] private Transform _source; // Chicken
		[SerializeField] private bool _isActive = true;
		[SerializeField] private float _weight = 1f;
		
		[Header("Position Settings")]
		[SerializeField] private Vector3 _positionAtRest = new Vector3(-14.54f, 0f, -10f);
		[SerializeField] private Vector3 _positionOffset = new Vector3(3.080001f, -0.33f, -10f);
		[SerializeField] private bool _freezePositionX = true;
		[SerializeField] private bool _freezePositionY = false;
		[SerializeField] private bool _freezePositionZ = true;
		
		[Header("Rotation Settings")]
		[SerializeField] private Vector3 _rotationAtRest = Vector3.zero;
		[SerializeField] private Vector3 _rotationOffset = Vector3.zero;
		[SerializeField] private bool _freezeRotationX = true;
		[SerializeField] private bool _freezeRotationY = true;
		[SerializeField] private bool _freezeRotationZ = true;

		private void LateUpdate()
		{
			if (!_isActive || _source == null || _weight <= 0f)
				return;

			UpdatePosition();
			UpdateRotation();
		}

		private void UpdatePosition()
		{
			var targetPosition = transform.position;
			var sourcePosition = _source.position;
			
			var constrainedPosition = sourcePosition + _positionOffset;
			
			if (!_freezePositionX)
			{
				targetPosition.x = Mathf.Lerp(
					_positionAtRest.x,
					constrainedPosition.x,
					_weight
				);
			}
			else
			{
				targetPosition.x = _positionAtRest.x;
			}
			
			if (!_freezePositionY)
			{
				targetPosition.y = Mathf.Lerp(
					_positionAtRest.y,
					constrainedPosition.y,
					_weight
				);
			}
			else
			{
				targetPosition.y = _positionAtRest.y;
			}
			
			if (!_freezePositionZ)
			{
				targetPosition.z = Mathf.Lerp(
					_positionAtRest.z,
					constrainedPosition.z,
					_weight
				);
			}
			else
			{
				targetPosition.z = _positionAtRest.z;
			}
			
			transform.position = targetPosition;
		}

		private void UpdateRotation()
		{
			if (_freezeRotationX && _freezeRotationY && _freezeRotationZ)
			{
				transform.rotation = Quaternion.Euler(_rotationAtRest + _rotationOffset);
				return;
			}

			var sourceRotation = _source.rotation * Quaternion.Euler(_rotationOffset);
			var eulerAtRest = _rotationAtRest;
			var eulerSource = sourceRotation.eulerAngles;
			
			var finalEuler = new Vector3(
				_freezeRotationX ? eulerAtRest.x : Mathf.LerpAngle(eulerAtRest.x, eulerSource.x, _weight),
				_freezeRotationY ? eulerAtRest.y : Mathf.LerpAngle(eulerAtRest.y, eulerSource.y, _weight),
				_freezeRotationZ ? eulerAtRest.z : Mathf.LerpAngle(eulerAtRest.z, eulerSource.z, _weight)
			);
			
			transform.rotation = Quaternion.Euler(finalEuler);
		}

		public void SetActive(bool isActive)
		{
			_isActive = isActive;
		}

		public void SetWeight(float weight)
		{
			_weight = Mathf.Clamp01(weight);
		}

		public void SetSource(Transform source)
		{
			_source = source;
		}
	}
}

