using CI.General;
using UnityEngine;

namespace Tara.AI.MovementStates
{
	public class Roaming : State<AIMovement>
	{
		private Movement _movement;
		
		private Vector2 _destination;

		public Roaming(AIMovement stateMachine) : base(stateMachine)
		{
			_movement = StateMachine.Movement;
		}

		public override void Start()
		{
			StateMachine.TargetPosition = GenerateRandomLocation();
			
			_movement.EnableMovement = true;
			_movement.EnableRotation = true;

			StateMachine.SetState(new FollowPath(StateMachine));
		}

		public override void Update()
		{
			_movement.MoveInput = _destination.AsVector3();
		}

		private void SetStateToIdle()
		{
			StateMachine.SetState(new Idle(StateMachine));
		}

		private Vector2 GenerateRandomLocation()
		{
			var x = Random.Range(-100f, 100f);
			var y = Random.Range(-100f, 100f);
			return new Vector2(x, y);
		}
	}
}
