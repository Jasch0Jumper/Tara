using UnityEngine;

namespace Tara
{
	public class PlayerInput : MonoBehaviour, IMoveInput, IRotateInput, IShootInput
	{
		private float horizontal;
		private float vertical;
		private Vector2 input;

		private float speedMultiplier;

		private bool isShooting;

		private void Update()
		{
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");

			input = new Vector2(horizontal, vertical);

			speedMultiplier = 1f;

			if (Input.GetMouseButton(0)) { isShooting = true; }
			else { isShooting = false; }
		}

		public Vector2 GetInput()
		{
			return input;
		}

		public float GetSpeedMultiplier()
		{
			return speedMultiplier;
		}

		public Vector2 GetTargetRotationPosition()
		{
			return Vector2.zero;
		}

		public bool LookAtMouse()
		{
			return true;
		}

		public bool IsShooting()
		{
			return isShooting;
		}
	}
}