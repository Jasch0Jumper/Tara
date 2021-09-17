using System;
using System.Collections.Generic;
using Tara.Pathfinding;
using UnityEngine;

namespace Tara.AI.MovementStates
{
	public class FollowPath : State<AIMovement>
	{
		private Movement _movement;
		private PathFinderBehavior _pathFinder;

		private Stack<Vector3> _path = new Stack<Vector3>();

		public FollowPath(AIMovement stateMachine) : base(stateMachine)
		{
			_movement = StateMachine.Movement;
			_pathFinder = StateMachine.PathFinder;
		}

		public override void Start()
		{
			_path = _pathFinder.FindPathTo(StateMachine.TargetPosition);
		}

		public override void Update()
		{
			var currentTargetPos = _path.Peek();

			MoveTo(currentTargetPos);

			if (Vector3.Distance(_movement.transform.position, currentTargetPos) < 5f)
				_path.Pop();

			if (_path.Count < 1)
				RevertToPreviousState();
		}

		private void MoveTo(Vector3 position)
		{
			var delta = position - _movement.transform.position;
			_movement.MoveInput = delta;
			_movement.LookAtPosition = position;
		}
	}
}
