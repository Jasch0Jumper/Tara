namespace Tara.AI
{
	public abstract class State<T>
	{
		protected StateMachine<T> StateMachine { get; private set; }
		protected T Reference { get => StateMachine.Reference; }

		public State(StateMachine<T> stateMachine)
		{
			StateMachine = stateMachine;
		}

		public virtual void Start()
		{

		}
		public virtual void Update()
		{

		}
	}
}
