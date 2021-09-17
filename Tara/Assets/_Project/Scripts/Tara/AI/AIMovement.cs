using UnityEngine;
using Tara.AI.MovementStates;

namespace Tara.AI
{
	[RequireComponent(typeof(Movement))]
	public class AIMovement : StateMachine<Movement>
	{
		private void Awake()
		{
			Reference = GetComponent<Movement>();
		}

		private void Start()
		{
			SetState(new Roaming(this));
		}

		private void Update()
		{
			State.Update();
		}
	}
}
