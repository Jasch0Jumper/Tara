using UnityEngine;

namespace Tara.AI
{
	public abstract class StateMachine<T> : MonoBehaviour where T: StateMachine<T>
	{
		protected State<T> State { get; private set; }
		
		protected abstract State<T> DefaultState { get; }

		public void SetState(State<T> state)
		{
			State = state;
			state.Start();
		}

		public void ReturnToDefaultState()
		{
			SetState(DefaultState);
		}
	}
}
