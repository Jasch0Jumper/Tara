using UnityEngine;
using UnityEngine.InputSystem;

namespace Tara
{
	public class PlayerMovement : Movement
	{
		private Controls _controls;

		[SerializeField] private string defaultControlScheme = "Keyboard&Mouse";

		private new void Awake()
		{
			base.Awake();
			_controls = new Controls();
		}

		private void OnEnable() => _controls.Player.Enable();
		private void OnDisable() => _controls.Player.Disable();

		private void Update()
		{
			MovementInput = _controls.Player.Move.ReadValue<Vector2>();
			RotationTargetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		}
	}
}