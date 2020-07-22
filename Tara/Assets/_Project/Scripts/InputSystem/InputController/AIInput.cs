using UnityEngine;
using Tara.PathfindingSystem;

namespace Tara.InputSystem
{
	[RequireComponent(typeof(IAIInput))]
	public class AIInput : MonoBehaviour, IRotateInput, IMoveInput, IShootInput
	{
		[SerializeField] [Range(1f, 5f)] private float fastSpeedMultiplier = 1f;
		[SerializeField] [Range(0.1f, 1f)] private float slowSpeedMultiplier = 1f;
		
		private PathFinderBehavior _pathFinder;
		private IAIInput _aIInput;
		
		private Vector2 _input;
		private Vector2 _targetRotationPosition;

		private void Awake()
		{
			_aIInput = GetComponent<IAIInput>();
			_pathFinder = GetComponent<PathFinderBehavior>();
		}

		private void Update()
		{
			if (_aIInput.UseFastSpeed()) { SpeedMultiplier = fastSpeedMultiplier; }
			else if (_aIInput.UseSlowSpeed()) { SpeedMultiplier = slowSpeedMultiplier; }
			else { SpeedMultiplier = 1f; }

			Vector3 aiTargetPosition = _aIInput.GetTargetPosition();
			Vector3 pathFindingTargetPosition = _pathFinder.PathFindTo(aiTargetPosition);

			Vector2 targetInput;

			if (IsInLineOfSight(aiTargetPosition))
			{
				_targetRotationPosition = aiTargetPosition;
				targetInput = aiTargetPosition - transform.position;
			}
			else
			{
				_targetRotationPosition = pathFindingTargetPosition;
				targetInput = pathFindingTargetPosition;
			}

			if (_aIInput.HasArrived())
			{ _input = Vector2.zero; }
			else
			{ _input = Vector2.ClampMagnitude(targetInput, 1f); }

			if (_aIInput.CanShoot()) { IsShooting = true; }
			else { IsShooting = false; }
		}

		private bool IsInLineOfSight(Vector3 target)
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, target);
			if (raycastHit2D.transform.position == target)
			{
				Debug.Log("True", this); return true;
			}
			Debug.Log("False", this); return false;
		}

		#region Interface implementations
		public Vector2 Input => _input;
		public Vector2 TargetRotationPosition => _targetRotationPosition;

		public float SpeedMultiplier { get; private set; } = 1f;

		public bool LookAtMouse() => false;

		public bool IsShooting { get; private set; }
		#endregion
	}
}