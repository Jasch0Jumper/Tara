using UnityEngine;

namespace Tara.AI.MovementStates
{
	public class Roaming : State<AIMovement>
	{
		private Movement _movement;

		public Roaming(AIMovement stateMachine) : base(stateMachine)
		{
			_movement = StateMachine.Movement;
		}

		public override void Start()
		{
			var target = FindClosestTarget();

			if (target is null)
			{
				StateMachine.SwitchTo<Idle>();
				return;
			}

			_movement.EnableMovement = true;
			_movement.EnableRotation = true;

			StateMachine.SwitchTo<FollowPath>();
		}

		private Entity FindClosestTarget()
		{
			var hits = StateMachine.CheckSoroundings();

			Entity target = null;

			float shortestDistance = float.PositiveInfinity;

			foreach (var hit in hits)
			{
				var entity = hit.collider.GetComponent<Entity>();
				if (entity == null) continue;

				if (hit.distance < shortestDistance)
				{
					target = entity;
					StateMachine.TargetPosition = hit.point;
				}
			}

			return target;
		}

		public override void Update()
		{
			if (Vector3.Distance(_movement.transform.position, StateMachine.TargetPosition) < 5f)
			{
				StateMachine.SwitchTo<Idle>();
				return;
			}

			_movement.MoveTo(StateMachine.TargetPosition);
		}
	}
}
