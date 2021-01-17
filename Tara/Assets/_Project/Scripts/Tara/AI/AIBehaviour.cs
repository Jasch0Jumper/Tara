namespace Tara.AI
{
	public abstract class AIBehaviour
	{
		protected CombatState CombatState;
		protected MovementState MovementState;

		public void SetCombatState(CombatState state) => CombatState = state;
		public void SetMovementState(MovementState state) => MovementState = state;
	}
}

