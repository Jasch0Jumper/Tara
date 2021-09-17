using CI.Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace Tara.Pathfinding
{
	public class PathFinderBehavior : MonoBehaviour
	{
		private GridBehaviour _gridBehaviour;
		private PathFinder _pathFinder;

		public Stack<Vector3> Path = new Stack<Vector3>();

		public Stack<Vector3> FindPathTo(Vector3 position)
		{
			GetReferences();

			var startNode = _gridBehaviour.GetNodeAt(transform.position);
			var destinationNode = _gridBehaviour.GetNodeAt(position);

			var nodes = _pathFinder.GetPath(startNode, destinationNode);

			if (nodes is null)
			{
				nodes = new Stack<GridNode>(new GridNode[] { startNode });
			}

			var path = NodesToVectors(nodes);

			Path = path;

			return path;
		}

		private void GetReferences()
		{
			_gridBehaviour = FindObjectOfType<GridBehaviour>();
			_pathFinder = new PathFinder(_gridBehaviour.Grid);
		}

		private Stack<Vector3> NodesToVectors(Stack<GridNode> nodes)
		{
			var nodeList = new List<GridNode>(nodes);
			nodeList.Reverse();

			var path = new Stack<Vector3>();

			foreach (var node in nodeList)
			{
				path.Push(_gridBehaviour.GridToGlobal(node.Position));
			}

			return path;
		}

		[Header("Debug")]
		[SerializeField] private Vector3 destination;

		[ContextMenu("GetPath")]
		private void GeneratePath()
		{
			GetReferences();

			FindPathTo(destination);
		}

		#region Gizmos
#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			if (Path.Count <= 1) return;

			DrawLines();
		}

		private void DrawLines()
		{
			var nodes = Path.ToArray();
			for (int i = 0; i < nodes.Length - 1; i++)
			{
				int next = i + 1;
				Gizmos.color = Color.yellow;

				if (i >= nodes.Length - 1)
				{ next = 0; }

				Gizmos.DrawLine(nodes[i], nodes[next]);
			}
		}
#endif
		#endregion
	}
}
