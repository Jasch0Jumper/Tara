namespace Tara.AI
{
	public abstract class State<T> where T: StateMachine<T>
	{
		protected T StateMachine { get; private set; }

		public State(T stateMachine)
		{
			StateMachine = stateMachine;
		}
		public State() { }

		public virtual void Start()
		{

		}
		public virtual void Update()
		{

		}

		protected void SetStateTo<TState>() where TState : State<T>, new()
		{
			var newState = new TState();
			newState.StateMachine = StateMachine;
			StateMachine.SetState(newState);
		}
	}
}
