namespace Tara.AI.MovementStates
{
	public class FollowPath : State<Movement>
	{
		public FollowPath(StateMachine<Movement> stateMachine, Movement reference) : base(stateMachine, reference)
		{
		}
	}
}
