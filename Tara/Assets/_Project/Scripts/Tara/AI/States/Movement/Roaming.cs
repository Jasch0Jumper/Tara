using CI.General;
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
			var target = FindClosestEntity();

			SetStateTo<Idle>();

			if (target is null) StateMachine.SetState(new Idle(StateMachine));

			StateMachine.TargetPosition = target.transform.position;


			_movement.EnableMovement = true;
			_movement.EnableRotation = true;

			StateMachine.SetState(new FollowPath(StateMachine));
		}

		private Entity FindClosestEntity()
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
				}
			}

			return target;
		}

		public override void Update()
		{
			_movement.MoveInput = StateMachine.TargetPosition;
		}
	}
}
