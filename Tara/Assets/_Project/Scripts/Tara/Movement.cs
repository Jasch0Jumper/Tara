using UnityEngine;
using CI.Utilities;

namespace Tara
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Movement : MonoBehaviour
	{
		[Range(0f, 50f)] public float MoveSpeed;
		[Space]
		[Range(0f, 50f)] public float RotationSpeed = 5f;
		[Range(-179f, 179f)] public float RotationOffset = -90f;

		public Vector2 MoveInput { get; set; }
		public Vector2 LookAtPosition { get; set; }

		public bool EnableMovement { get; set; }
		public bool EnableRotation { get; set; }

		private Rigidbody2D _rigidbody;

		protected void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
		}
		private void FixedUpdate()
		{
			if (EnableMovement)
				Move(MoveInput);

			if (EnableRotation)
				LookAt(LookAtPosition);
		}

		public void MoveTo(Vector3 position)
		{
			var delta = position - transform.position;
			MoveInput = delta;
			LookAtPosition = position;
		}

		private void Move(Vector2 input)
		{
			Vector2 newPosition = transform.position + input.ToVector3().normalized * (MoveSpeed * Time.fixedDeltaTime);
			_rigidbody.MovePosition(newPosition);
		}

		private void LookAt(Vector2 targetPosition)
		{
			var targetRotation = GetTargetRotation(targetPosition);
			
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed);
		}

		private Quaternion GetTargetRotation(Vector2 targetPosition)
		{
			Vector2 difference = targetPosition.ToVector3() - transform.position;
			float rotationAngel = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

			Quaternion targetRotation = Quaternion.Euler(0f, 0f, rotationAngel + RotationOffset);

			return targetRotation;
		}
	}
}
