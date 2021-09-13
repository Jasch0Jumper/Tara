using UnityEngine;

namespace Tara.AI
{
	public abstract class StateMachine<T> : MonoBehaviour
	{
		protected State<T> State { get; private set; }

		public void SetState(State<T> state)
		{
			State = state;
			state.Start();
		}
	}
}
