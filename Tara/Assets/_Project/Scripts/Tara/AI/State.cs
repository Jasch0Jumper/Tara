namespace Tara.AI
{
	public abstract class State<T>
	{
		protected StateMachine<T> StateMachine { get; private set; }
		protected T Reference { get; private set; }

		protected State(StateMachine<T> stateMachine, T reference)
		{
			StateMachine = stateMachine;
			Reference = reference;
		}

		public virtual void Start()
		{

		}
		public virtual void Update()
		{

		}
	}
}
