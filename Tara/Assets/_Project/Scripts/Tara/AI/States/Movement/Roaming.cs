using CI.General;
using UnityEngine;

namespace Tara.AI.MovementStates
{
	public class Roaming : State<Movement>
	{
		private Timer _timer;

		private Vector2 _direction;

		public Roaming(StateMachine<Movement> stateMachine, Movement reference) : base(stateMachine, reference)
		{
		}

		public override void Update()
		{
			Reference.MoveInput = _direction;

			_timer.Tick(Time.deltaTime);
		}

		public override void Start()
		{
			_timer = new Timer(Random.Range(1f, 10f));
			_timer.OnTimerEnd += SetStateToIdle;

			_direction = GenerateRandomDirection();
			Reference.RotationTargetPosition = Reference.transform.position + _direction.AsVector3();
		}
		
		private void SetStateToIdle()
		{
			StateMachine.SetState(new Idle(StateMachine, Reference));
		}

		private Vector2 GenerateRandomDirection()
		{
			var x = Random.Range(-1f, 1f);
			var y = Random.Range(-1f, 1f);
			return new Vector2(x, y);
		}
	}
}
