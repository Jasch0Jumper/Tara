using UnityEngine;

namespace Tara
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Movement : MonoBehaviour
	{
		public float DefaultMoveSpeed = 1f;
		[Space]
		public float DefaultRotationSpeed = 5f;
		[Range(-179f, 179f)] public float RotationOffset = -90f;

		private Rigidbody2D _rigidbody;

		protected Vector2 MovementInput;
		protected Vector2 RotationTargetPosition;

		protected void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
		}
		private void FixedUpdate()
		{
			Move(MovementInput);
			RotateTowards(RotationTargetPosition);
		}

		public void Move(Vector2 input, float speedMultiplier)
		{
			Vector2 targetPosition = transform.position + input.AsVector3() * DefaultMoveSpeed * speedMultiplier * Time.fixedDeltaTime;

			_rigidbody.MovePosition(targetPosition);
		}
		public void Move(Vector2 input) => Move(input, 1);

		public void RotateTowards(Vector2 targetPosition, float rotationSpeedMultiplier)
		{
			var targetRotation = GetTargetRotation(targetPosition);
			
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, DefaultRotationSpeed * rotationSpeedMultiplier);
		}
		public void RotateTowards(Vector2 targetPosition) => RotateTowards(targetPosition, 1f);

		private Quaternion GetTargetRotation(Vector2 targetPosition)
		{
			Vector2 difference = targetPosition.AsVector3() - transform.position;
			float rotationAngel = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

			Quaternion targetRotation = Quaternion.Euler(0f, 0f, rotationAngel + RotationOffset);

			return targetRotation;
		}
	}
}
