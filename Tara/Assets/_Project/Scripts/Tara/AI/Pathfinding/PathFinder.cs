﻿using System.Collections.Generic;

namespace Tara.Pathfinding
{
	public class PathFinder
	{
		private int LIMIT = 1000;

		private GridBehaviour _grid;

		public PathFinder(GridBehaviour grid)
		{
			_grid = grid;
		}

		public Stack<GridNode> GetPath(GridNode start, GridNode destination)
		{
			List<PathNode> openList = new List<PathNode>();
			List<PathNode> closedList = new List<PathNode>();

			var startNode = new PathNode(start, destination);
			var destinationNode = new PathNode(destination, destination);

			openList.Add(startNode);

			do
			{
				var currentNode = GetNodeWithLowestFScore(openList);

				closedList.Add(currentNode);
				openList.Remove(currentNode);

				if (closedList.Contains(destinationNode)) break; 

				var adjacentNodes = GetWalkableAdjacentNodes(currentNode);

				foreach (var node in adjacentNodes)
				{
					if (closedList.Contains(node)) continue;

					UpdateParentIfFCostWillBeCheaper(currentNode, node);

					if (!openList.Contains(node))
					{
						openList.Add(node);
					}
				}
			}
			while (openList.Count > 0 && openList.Count < LIMIT);

			return CalculateFinalPathStack(closedList[closedList.IndexOf(destinationNode)]);
		}

		private static void UpdateParentIfFCostWillBeCheaper(PathNode currentNode, PathNode node)
		{
			var oldFScore = node.FScore;
			var oldParent = node.Parent;

			node.SetParent(currentNode);

			if (node.FScore > oldFScore)
			{
				node.SetParent(oldParent);
			}
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

		private Stack<GridNode> CalculateFinalPathStack(PathNode endNode)
		{
			var finishedPath = new Stack<GridNode>();

			var currentNode = endNode;

			do
			{
				finishedPath.Push(currentNode.GridNode);
				currentNode = currentNode.Parent;
			}
			while (currentNode != null);

			return finishedPath;
		}
	}
}
