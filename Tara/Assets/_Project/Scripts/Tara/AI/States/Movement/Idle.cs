using CI.General;
using UnityEngine;

namespace Tara.AI.MovementStates
{
	public class Idle : State<AIMovement>
	{
		private Timer _timer;

		public Idle(AIMovement stateMachine) : base(stateMachine)
		{
		}

		public override void Start()
		{
			StateMachine.Movement.EnableMovement = false;

			_timer = new Timer(Random.Range(2f, 10f));
			_timer.OnTimerEnd += SetStateToRoaming;

		}
		public override void Update()
		{
			_timer.Tick(Time.deltaTime);
		}

		private void SetStateToRoaming()
		{
			StateMachine.SwitchTo<Roaming>();
		}
	}
}
