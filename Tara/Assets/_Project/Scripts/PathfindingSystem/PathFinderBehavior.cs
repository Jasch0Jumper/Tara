using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class PathFinderBehavior : MonoBehaviour
	{
		[SerializeField] [Range(1f, 20f)] float tollerance = default;

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
			if (_pathNodes.Count < 1 || Vector3.Distance(_currentLongTermDestination, position) > tollerance)
			{
				_pathNodes = _pathFinder.GetPath(_grid.GetCell(transform.position), _grid.GetCell(position));
				_currentLongTermDestination = position;
				
				_currentDestination = _pathNodes.Pop().Position;	
			}

			if (Vector3.Distance(transform.position, _currentDestination) < tollerance)
			{
				_currentDestination = _pathNodes.Pop().Position;
			}

			return _currentDestination;
		}

		//[SerializeField] private Vector3 pos1 = default;
		//[SerializeField] private Vector3 pos2 = default;

		//[ContextMenu("GetPath")]
		//private void GeneratePath()
		//{
		//	PathNode startNode = _grid.GetCell(pos1);
		//	PathNode destinationNode = _grid.GetCell(pos2);

		//	pathNodes = _pathFinder.GetPath(startNode, destinationNode);
		//}

		#region Gizmos
		private void OnDrawGizmosSelected()
		{
			//Gizmos.DrawWireSphere(pos1, 2.5f);
			//Gizmos.DrawWireSphere(pos2, 2.5f);

			if (_pathNodes.Count <= 1)
			{ return; }

			DrawLines();
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
		#endregion
	}
}
