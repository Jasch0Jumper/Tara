using CI.General;
using UnityEngine;

namespace Tara.AI.MovementStates
{
	public class Roaming : State<Movement>
	{
		private Timer _timer;

		private Vector2 _direction;

		public Roaming(StateMachine<Movement> stateMachine) : base(stateMachine)
		{
		}

		public override void Start()
		{
			_timer = new Timer(Random.Range(1f, 10f));
			_timer.OnTimerEnd += SetStateToIdle;

			_direction = GenerateRandomDirection();
			Reference.LookAtPosition = Reference.transform.position + _direction.AsVector3();

			Reference.EnableMovement = true;
			Reference.EnableRotation = true;
		}

		public override void Update()
		{
			Reference.MoveInput = _direction.AsVector3();

			_timer.Tick(Time.deltaTime);
		}

		private void SetStateToIdle()
		{
			StateMachine.SetState(new Idle(StateMachine));
		}

		private Vector2 GenerateRandomDirection()
		{
			var x = Random.Range(-1f, 1f);
			var y = Random.Range(-1f, 1f);
			return new Vector2(x, y);
		}
	}
}
