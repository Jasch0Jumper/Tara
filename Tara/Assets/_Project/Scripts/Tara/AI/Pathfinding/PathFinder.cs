using System.Collections.Generic;
using UnityEngine;
using CITools;

namespace Tara.Pathfinding
{
	public class PathFinder
	{
		private GridBehaviour _grid;

		public PathFinder(GridBehaviour grid)
		{
			_grid = grid;
		}

		public Stack<GridNode> GetPath(GridNode start, GridNode destination)
		{
			List<PathNode> openList = new List<PathNode>();
			List<GridNode> closedList = new List<GridNode>();

			var startNode = new PathNode(start, destination);
			
			openList.Add(startNode);

			do
			{
				var currentNode = GetNodeWithLowestFScore(openList);

				closedList.Add(currentNode.GridNode);
				openList.Remove(currentNode);

				if (closedList.Contains(destination)) break; 

				var adjacentNodes = GetWalkableAdjacentNodes(currentNode);

				foreach (var node in adjacentNodes)
				{
					if (closedList.Contains(node.GridNode)) continue;

					if (openList.Contains(node) == false)
					{
						node.SetParent(currentNode);

						openList.Add(node);
					}
					else
					{
						var oldFScore = node.FScore;
						var oldParent = node.ParentNode;
						
						node.SetParent(currentNode);

						if (node.FScore >= oldFScore)
						{
							node.SetParent(oldParent);
						}
					}
				}
			}
			while (openList.Count > 0);

			return ConvertToFinishedPathStack(closedList);
		}

		private PathNode GetNodeWithLowestFScore(List<PathNode> nodes)
		{
			PathNode nodeWithLowestScore = nodes[0];

			foreach (PathNode node in nodes)
			{
				if (node.FScore < nodeWithLowestScore.FScore)
				{
					nodeWithLowestScore = node;
				}
			}

			return nodeWithLowestScore;
		}

		private List<PathNode> GetWalkableAdjacentNodes(PathNode parent)
		{
			var adjacentNodes = _grid.GetNodesAround(parent.GridNode);
			var pathNodes = new List<PathNode>();

			foreach (var node in adjacentNodes)
			{
				if (node.Walkable == false) continue;
				
				var newNode = new PathNode(node, parent);

				pathNodes.Add(newNode);
			}

			return pathNodes;
		}

		private Stack<GridNode> ConvertToFinishedPathStack(List<GridNode> pathNodes)
		{
			var finishedPath = new Stack<GridNode>();
			
			pathNodes.Reverse();
			
			for (int i = 0; i < pathNodes.Count; i++)
			{
				finishedPath.Push(pathNodes[i]);
			}

			return finishedPath;
		}
	}
}
