using UnityEngine;

namespace Tara
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Movement : MonoBehaviour
	{
		public float MoveSpeed = 10f;
		[Space]
		public float RotationSpeed = 5f;
		[Range(-179f, 179f)] public float RotationOffset = -90f;

		public Vector2 MoveInput { get; set; }
		public Vector2 RotationTargetPosition { get; set; }

		private Rigidbody2D _rigidbody;

		protected void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
		}
		private void FixedUpdate()
		{
			Move(MoveInput, MoveSpeed);
			RotateTowards(RotationTargetPosition, RotationSpeed);
		}

		private void Move(Vector2 input, float moveSpeed)
		{
			Vector2 targetPosition = transform.position + input.AsVector3().normalized * (moveSpeed * Time.fixedDeltaTime);

			_rigidbody.MovePosition(targetPosition);
		}

		private void RotateTowards(Vector2 targetPosition, float rotationSpeed)
		{
			var targetRotation = GetTargetRotation(targetPosition);
			
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
		}

		private Quaternion GetTargetRotation(Vector2 targetPosition)
		{
			Vector2 difference = targetPosition.AsVector3() - transform.position;
			float rotationAngel = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

			Quaternion targetRotation = Quaternion.Euler(0f, 0f, rotationAngel + RotationOffset);

			return targetRotation;
		}
	}
}
