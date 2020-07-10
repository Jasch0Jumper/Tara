using UnityEngine;
using Tara.InputSystem;

namespace Tara
{
	[RequireComponent(typeof(Rigidbody2D), typeof(IMoveInput))]
	public class Movement : MonoBehaviour
	{
		[SerializeField] private float moveSpeed = 1f;

		private Rigidbody2D _rigidbody;
		private IMoveInput _inputController;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_inputController = GetComponent<IMoveInput>();
		}

		private void FixedUpdate()
		{
			Move(_inputController.Input, _inputController.SpeedMultiplier);
		}

		private void Move(Vector3 input, float speedMultiplier)
		{
			Vector2 targetPosition = transform.position + input * moveSpeed * speedMultiplier * Time.fixedDeltaTime;

			_rigidbody.MovePosition(targetPosition);
		}
	}
}