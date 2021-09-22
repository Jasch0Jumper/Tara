using UnityEngine;

namespace Tara.AI
{
	public abstract class StateMachine<T> : MonoBehaviour where T: StateMachine<T>
	{
		protected State<T> State { get; private set; }
		
		protected abstract State<T> DefaultState { get; }

		public void SwitchTo<TState>() where TState : State<T>
		{
			object[] args = { this };
			var newState = (TState)System.Activator.CreateInstance(typeof(TState), args);

			SetState(newState);
		}

		public void SwitchToDefaultState()
		{
			SetState(DefaultState);
		}

		private void SetState(State<T> state)
		{
			State = state;
			state.Start();
		}
	}
}
