using System.Collections.Generic;
using UnityEngine;

namespace Tara.Pathfinding
{
	public class PathFinderBehavior : MonoBehaviour
	{
		private GridBehaviour _grid;
		private PathFinder _pathFinder;

		private List<Vector3> _path = new List<Vector3>();

		public Stack<Vector3> FindPathTo(Vector3 position)
		{
			GetReferences();

			var nodes = _pathFinder.GetPath(_grid.GetNodeAt(transform.position), _grid.GetNodeAt(position));
			
			var path = NodesToVectors(nodes);

			//_path = path;

			return path;
		}

		private void GetReferences()
		{
			_grid = FindObjectOfType<GridBehaviour>();
			_pathFinder = new PathFinder(_grid);
		}

		private static Stack<Vector3> NodesToVectors(Stack<GridNode> nodes)
		{
			var nodeList = new List<GridNode>(nodes);
			nodeList.Reverse();

			var path = new Stack<Vector3>();

			foreach (var node in nodeList)
			{
				path.Push(node.WorldPosition);
			}

			return path;
		}

		[Header("Gizmos")]
		[SerializeField] private Vector3 pos1;
		[SerializeField] private Vector3 pos2;

		[ContextMenu("GetPath")]
		private void GeneratePath()
		{
			GetReferences();

			GridNode startNode = _grid.GetNodeAt(pos1);

			GridNode destinationNode = _grid.GetNodeAt(pos2);

			var path = _pathFinder.GetPath(startNode, destinationNode);

			_path = new List<Vector3>(NodesToVectors(path));
		}
		[ContextMenu("Pop")]
		private void PopPath()
		{
			if (_path.Count == 0) return;
			_path.RemoveAt(0);
		}


		#region Gizmos
#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(pos1, 2.5f);
			Gizmos.DrawWireSphere(pos2, 2.5f);

			if (_path.Count <= 1) return;

			DrawLines();
		}

		private void DrawLines()
		{
			var nodes = _path.ToArray();
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
