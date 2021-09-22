namespace Tara.AI
{
	public abstract class State<T> where T: StateMachine<T>
	{
		protected T StateMachine { get; private set; }

		public State(T stateMachine)
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
