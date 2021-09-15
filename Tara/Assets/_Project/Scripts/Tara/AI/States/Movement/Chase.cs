namespace Tara.AI.MovementStates
{
	public class Chase : State<Movement>
	{
		public Chase(StateMachine<Movement> stateMachine, Movement reference) : base(stateMachine, reference)
		{
		}
	}
}
