using UnityEngine;
using Tara.InputSystem;

namespace Tara
{
	[RequireComponent(typeof(IRotateInput))]
	public class Rotation : MonoBehaviour
	{
		[SerializeField] [Range(-179f, 179f)] private float rotationOffset = 0f;

		[Tooltip("Can be ignored if Object is looking at Mouse.")]
		[SerializeField] private float maxRotationPerFrame = 1f;

		private IRotateInput _rotateInput;

		private void Awake()
		{
			_rotateInput = GetComponent<IRotateInput>();
		}

		private void Update()
		{
			if (_rotateInput.LookAtMouse()) { LookAtMouse(); }
			else { LookAtTarget(_rotateInput.GetTargetRotationPosition(), maxRotationPerFrame); }
		}

		private void LookAtTarget(Vector3 targetPosition, float maxDegreesDelta)
		{
			var targetRotation = GetTargetRotation(targetPosition);

			transform.rotation = Quaternion.RotateTowards(CurrentRotation(), targetRotation, maxDegreesDelta);
		}

		private void LookAtMouse()
		{
			var targetRotation = GetTargetRotation(Camera.main.ScreenToWorldPoint(Input.mousePosition));

			transform.rotation = Quaternion.Euler(0f, 0f, targetRotation.eulerAngles.z + rotationOffset);
		}

		private Quaternion GetTargetRotation(Vector3 targetPosition)
		{
			Vector2 difference = targetPosition - transform.position;
			float rotationAngel = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

			Quaternion targetRotation = Quaternion.Euler(0f, 0f, rotationAngel + rotationOffset);

			return targetRotation;
		}

		private Quaternion CurrentRotation()
		{
			Quaternion currentRotation = transform.rotation;
			return currentRotation;
		}
	}
}