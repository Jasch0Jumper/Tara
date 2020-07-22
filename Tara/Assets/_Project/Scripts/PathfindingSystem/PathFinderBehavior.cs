using System.Collections.Generic;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class PathFinderBehavior : MonoBehaviour
	{
		[SerializeField] float tollerance = default;

		private Grid<PathNode> _grid;
		private PathFinder _pathFinder;

		private Stack<PathNode> pathNodes = new Stack<PathNode>();
		private Vector3 _currentDestination;

		private void Start()
		{
			_grid = GridManager.Grid;
			_pathFinder = new PathFinder(GridManager.Grid);
		}


		//public Vector3 PathFindTo(Vector3 position)
		//{
		//	if (Vector3.Distance(position, _currentDestination) < tollerance) { return; }
		//}


		[SerializeField] private Vector3 pos1 = default;
		[SerializeField] private Vector3 pos2 = default;

		[ContextMenu("GetPath")]
		private void GeneratePath()
		{
			PathNode startNode = _grid.GetCell(pos1);
			PathNode destinationNode = _grid.GetCell(pos2);

			pathNodes = _pathFinder.GetPath(startNode, destinationNode);
		}


		#region Gizmos
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(pos1, 2.5f);
			Gizmos.DrawWireSphere(pos2, 2.5f);

			if (pathNodes.Count <= 1) { return; }

			PathNode[] nodes = pathNodes.ToArray();
			for (int i = 0; i < nodes.Length - 1; i++)
			{
				int indexPlusOne = i + 1;
				Gizmos.color = Color.yellow;

				if (i >= nodes.Length - 1) { indexPlusOne = 0; }

				Gizmos.DrawLine(nodes[i].Position, nodes[indexPlusOne].Position);
			}
		}

		#endregion

	}
}
