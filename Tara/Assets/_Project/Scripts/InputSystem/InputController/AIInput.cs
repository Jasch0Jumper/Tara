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

		private Vector3 _aiTargetPosition;
		private Vector3 _pathFindingTargetPosition;

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

			_aiTargetPosition = _aIInput.GetTargetPosition();
			_pathFindingTargetPosition = _pathFinder.PathFindTo(_aiTargetPosition);

			Vector2 targetInput;

			_targetRotationPosition = _pathFindingTargetPosition;
			targetInput = _pathFindingTargetPosition - transform.position;

			if (_aIInput.HasArrived())
			{ _input = Vector2.zero; }
			else
			{ _input = Vector2.ClampMagnitude(targetInput, 1f); }

			if (_aIInput.CanShoot()) { IsShooting = true; }
			else { IsShooting = false; }
		}

		#region Interface implementations
		public Vector2 Input => _input;
		public Vector2 TargetRotationPosition => _targetRotationPosition;

		public float SpeedMultiplier { get; private set; } = 1f;

		public bool LookAtMouse => false;

		public bool IsShooting { get; private set; }
		#endregion

		#region Gizmos
#if UNITY_EDITOR

		[Header("Gizmos")]
		[SerializeField] private bool showTargetPoints = default;

		private void OnDrawGizmos()
		{
			if (showTargetPoints) { DrawTargetPoints(); }
		}
		private void OnDrawGizmosSelected()
		{
			DrawTargetPoints();
		}

		private void DrawTargetPoints()
		{
			if (_aiTargetPosition == Vector3.zero || _pathFindingTargetPosition == Vector3.zero) { return; }

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(_aiTargetPosition, 2.5f);

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(_pathFindingTargetPosition, 2.5f);
		}

#endif
		#endregion
	}
}
