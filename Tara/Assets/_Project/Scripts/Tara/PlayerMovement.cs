using UnityEngine;
using UnityEngine.InputSystem;

namespace Tara
{
	[RequireComponent(typeof(Movement))]
	public class PlayerMovement : MonoBehaviour
	{
		private Movement _movement;
		private Controls _controls;

		private void Awake()
		{
			_movement = GetComponent<Movement>();
			_controls = new Controls();
		}

		private void Start()
		{
			_movement.EnableMovement = true;
			_movement.EnableRotation = true;
		}

		private void OnEnable() => _controls.Player.Enable();
		private void OnDisable() => _controls.Player.Disable();

		private void Update()
		{
			_movement.MoveInput = _controls.Player.Move.ReadValue<Vector2>();
			_movement.LookAtPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).AsVector2();
		}
	}
}