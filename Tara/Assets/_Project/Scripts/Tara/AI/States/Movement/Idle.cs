using CI.General;
using UnityEngine;

namespace Tara.AI.MovementStates
{
	public class Idle : State<Movement>
	{
		private Timer _timer;

		public Idle(StateMachine<Movement> stateMachine, Movement reference) : base(stateMachine, reference)
		{
		}

		public override void Start()
		{
			_timer = new Timer(Random.Range(2f, 10f));
			_timer.OnTimerEnd += SetStateToRoaming;

			Reference.MoveInput = Vector2.zero;
		}
		public override void Update()
		{
			_timer.Tick(Time.deltaTime);
		}

		private void SetStateToRoaming()
		{
			StateMachine.SetState(new Roaming(StateMachine, Reference));
		}
	}
}
