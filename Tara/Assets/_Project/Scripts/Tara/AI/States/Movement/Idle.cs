using CI.General;
using UnityEngine;

namespace Tara.AI.MovementStates
{
	public class Idle : State<Movement>
	{
		private Timer _timer;

		public Idle(StateMachine<Movement> stateMachine) : base(stateMachine)
		{
		}

		public override void Start()
		{
			_timer = new Timer(Random.Range(2f, 10f));
			_timer.OnTimerEnd += SetStateToRoaming;
		}
		public override void Update()
		{
			_timer.Tick(Time.deltaTime);
		}

		private void SetStateToRoaming()
		{
			StateMachine.SetState(new Roaming(StateMachine));
		}
	}
}
