using UnityEngine;

namespace Tara.AI
{
	public abstract class StateMachine<T> : MonoBehaviour where T: StateMachine<T>
	{
		protected State<T> State { get; private set; }
		
		public State<T> PreviousState { get; private set; }

		public void SetState(State<T> state)
		{
			PreviousState = State;
			State = state;
			state.Start();
		}
	}
}
