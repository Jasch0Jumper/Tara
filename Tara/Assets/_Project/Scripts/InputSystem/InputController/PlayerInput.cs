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
			_horizontal = UnityEngine.Input.GetAxis("Horizontal");
			_vertical = UnityEngine.Input.GetAxis("Vertical");

			_input = new Vector2(_horizontal, _vertical);

			_speedMultiplier = 1f;

			if (UnityEngine.Input.GetMouseButton(0)) { _isShooting = true; }
			else { _isShooting = false; }
		}

		public Vector2 Input => _input;

		public float SpeedMultiplier => _speedMultiplier;

		public Vector2 TargetRotationPosition => Vector2.zero;

		public bool LookAtMouse => true;

		public bool IsShooting => _isShooting;
	}
}