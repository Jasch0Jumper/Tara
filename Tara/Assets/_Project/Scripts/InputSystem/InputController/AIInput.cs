using UnityEngine;
using Tara.PathfindingSystem;

namespace Tara.InputSystem
{
	[RequireComponent(typeof(IAIInput))]
	public class AIInput : MonoBehaviour, IRotateInput, IMoveInput, IShootInput
	{
		[SerializeField] [Range(1f, 5f)] private float fastSpeedMultiplier = 1f;
		[SerializeField] [Range(0.1f, 1f)] private float slowSpeedMultiplier = 1f;
		private float _speedMultiplier = 1f;

		private PathFinder _pathFinder;
		private IAIInput _aIInput;

		private bool _isShooting;

		private Vector2 _input;
		private Vector2 _targetRotationPosition;

		private void Awake()
		{
			_aIInput = GetComponent<IAIInput>();
			_pathFinder = GetComponent<PathFinder>();
		}

		private void Update()
		{
			if (_aIInput.UseFastSpeed()) { _speedMultiplier = fastSpeedMultiplier; }
			else if (_aIInput.UseSlowSpeed()) { _speedMultiplier = slowSpeedMultiplier; }
			else { _speedMultiplier = 1f; }

			_targetRotationPosition = _aIInput.GetTargetPosition();

			//pathFinder.PathFindTo(aIInput.GetTargetPosition());

			Vector2 targetInput = _aIInput.GetTargetPosition() - transform.position;

			if (_aIInput.HasArrived())
			{ _input = Vector2.zero; }
			else
			{ _input = Vector2.ClampMagnitude(targetInput, 1f); }

			if (_aIInput.CanShoot()) { _isShooting = true; }
			else { _isShooting = false; }
		}

		#region Interface implementations

		public Vector2 GetInput()
		{
			return _input;
		}

		public Vector2 GetTargetRotationPosition()
		{
			return _targetRotationPosition;
		}

		public float GetSpeedMultiplier()
		{
			return _speedMultiplier;
		}

		public bool LookAtMouse()
		{
			return false;
		}

		public bool IsShooting()
		{
			return _isShooting;
		}

		#endregion 
	}
}