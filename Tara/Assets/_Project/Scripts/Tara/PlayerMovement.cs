using UnityEngine;
using UnityEngine.InputSystem;

namespace Tara
{
	public class PlayerMovement : Movement
	{
		private Controls _controls;

		private string _currentControlScheme;

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
			RotationTargetPosition = GetRotationTargetPosition();
		}

		public void OnControlsChanged(PlayerInput playerInput)
		{
			_currentControlScheme = playerInput.currentControlScheme;
		}

		private Vector2 GetRotationTargetPosition()
		{
			Vector2 center = new Vector2();

			switch (_currentControlScheme)
			{
				case "Keyboard&Mouse":
					//center = new Vector2(Screen.width / 2, Screen.height / 2);
					
					break;
				case "Gamepad":
					break;
				default:
					break;
			}

			return _controls.Player.Look.ReadValue<Vector2>().AsVector3() + center.AsVector3() + transform.position;
		}
	}
}