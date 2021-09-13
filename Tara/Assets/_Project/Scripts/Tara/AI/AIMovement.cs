using UnityEngine;
using Tara.AI.MovementStates;

namespace Tara.AI
{
	[RequireComponent(typeof(Movement))]
	public class AIMovement : StateMachine<Movement>
	{
		private Movement _movement;

		private void Awake()
		{
			_movement = GetComponent<Movement>();
		}

		private void Start()
		{
			SetState(new Roaming(this, _movement));
		}

		private void Update()
		{
			State.Update();
		}
	}
}
