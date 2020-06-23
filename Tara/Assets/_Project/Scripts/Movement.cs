using UnityEngine;
using Tara.InputSystem;

namespace Tara
{
	[RequireComponent(typeof(Rigidbody2D), typeof(IMoveInput))]
	public class Movement : MonoBehaviour
	{
		[SerializeField] private float moveSpeed = 1f;

		private Rigidbody2D rb;
		private IMoveInput inputController;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			inputController = GetComponent<IMoveInput>();
		}

		private void FixedUpdate()
		{
			Move(inputController.GetInput(), inputController.GetSpeedMultiplier());
		}

		private void Move(Vector3 input, float speedMultiplier)
		{
			Vector2 targetPosition = transform.position + input * moveSpeed * speedMultiplier * Time.fixedDeltaTime;

			rb.MovePosition(targetPosition);
		}
	}
}