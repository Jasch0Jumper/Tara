using UnityEngine;

namespace Tara.InputSystem
{
	public class PlayerInput : MonoBehaviour, IMoveInput, IRotateInput, IShootInput
	{
		private float _horizontal;
		private float _vertical;
		private Vector2 _input;

		private float _speedMultiplier;

		private bool _isShooting;

		private void Update()
		{
			_horizontal = Input.GetAxis("Horizontal");
			_vertical = Input.GetAxis("Vertical");

			_input = new Vector2(_horizontal, _vertical);

			_speedMultiplier = 1f;

			if (Input.GetMouseButton(0)) { _isShooting = true; }
			else { _isShooting = false; }
		}

		public Vector2 GetInput() => _input;

		public float GetSpeedMultiplier() => _speedMultiplier;

		public Vector2 GetTargetRotationPosition() => Vector2.zero;

		public bool LookAtMouse() => true;

		public bool IsShooting() => _isShooting;
	}
}