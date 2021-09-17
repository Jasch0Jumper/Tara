using UnityEngine;
using Tara.AI.MovementStates;
using Tara.Pathfinding;

namespace Tara.AI
{
	[RequireComponent(typeof(Movement), typeof(PathFinderBehavior))]
	public class AIMovement : StateMachine<AIMovement>
	{
		public Movement Movement { get; private set; }
		public PathFinderBehavior PathFinder { get; private set; }

		public Vector3 TargetPosition { get; set; }

		private void Awake()
		{
			Movement = GetComponent<Movement>();
			PathFinder = GetComponent<PathFinderBehavior>();
		}

		private void Start()
		{
			SetState(new Roaming(this));
		}

		private void Update()
		{
			State.Update();
		}


		#region Gizmos
#if UNITY_EDITOR

		[Header("Gizmos")]
		[SerializeField] private bool showTargetPosition;

		private void OnDrawGizmos()
		{
			if (showTargetPosition) DrawTargetPosition();
		}
		private void OnDrawGizmosSelected()
		{
			DrawTargetPosition();
		}

		private void DrawTargetPosition()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(TargetPosition, 2f);
		}
#endif
		#endregion

	}
}
