using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CITools;

namespace Tara.Pathfinding
{
	public class PathFinderBehavior : MonoBehaviour
	{
		[SerializeField] [Range(1f, 20f)] private float tollerance = default;
		[SerializeField] private LayerMask targets = default;

		private Grid<PathNode> _grid;
		private PathFinder _pathFinder;

		private Stack<PathNode> _pathNodes = new Stack<PathNode>();
		private Vector3 _currentDestination;
		private Vector3 _currentLongTermDestination;

		private void Start()
		{
			_grid = GridManager.Grid;
			_pathFinder = new PathFinder(GridManager.Grid);
		}

		public Vector3 PathFindTo(Vector3 position)
		{
			if (IsInLineOfSight(position)) { return position; }

			if (_pathNodes.Count <= 1)
			{
				_pathNodes = _pathFinder.GetPath(_grid.GetCellAt(transform.position), _grid.GetCellAt(position));

				PathNode[] nodes = _pathNodes.ToArray();

				_currentLongTermDestination = nodes[nodes.Length - 1].Position;

				_currentDestination = _pathNodes.Pop().Position;
			}

			float distanceToNextPathNode = Vector3.Distance(transform.position, _currentDestination);

			if (distanceToNextPathNode < tollerance && _pathNodes.Count > 0)
			{
				_currentDestination = _pathNodes.Pop().Position;
			}

			return _currentDestination;
		}

		private bool IsInLineOfSight(Vector3 target)
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, target, 500f, targets);
			if (Vector3.Distance(raycastHit2D.transform.position, target) < tollerance)
			{
				return true;
			}
			return false;
		}

		[Header("Gizmos")]
		[SerializeField] private Vector3 pos1 = default;
		[SerializeField] private Vector3 pos2 = default;

		[ContextMenu("GetPath")]
		private void GeneratePath()
		{
			PathNode startNode = _grid.GetCellAt(pos1);
			PathNode destinationNode = _grid.GetCellAt(pos2);

			_pathNodes = _pathFinder.GetPath(startNode, destinationNode);
		}

		#region Gizmos
#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(pos1, 2.5f);
			Gizmos.DrawWireSphere(pos2, 2.5f);

			if (_pathNodes.Count <= 1)
			{ return; }

			DrawLines();

			foreach (var node in _pathNodes)
			{
				Handles.Label(node.Position, $"F: {node.FScore}, G: {node.GScore}, H: {node.HScore}, WalkCost: {node.WalkCost}");
			}

			if (_currentLongTermDestination == Vector3.zero) { return; }
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(_currentLongTermDestination, 2.5f);
		}

		private void DrawLines()
		{
			PathNode[] nodes = _pathNodes.ToArray();
			for (int i = 0; i < nodes.Length - 1; i++)
			{
				int indexPlusOne = i + 1;
				Gizmos.color = Color.yellow;

				if (i >= nodes.Length - 1)
				{ indexPlusOne = 0; }

				Gizmos.DrawLine(nodes[i].Position, nodes[indexPlusOne].Position);
			}
		}
#endif
		#endregion
	}
}
