using System.Collections.Generic;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class PathFinder
	{
		private int _normalGCost = 10;
		private int _diagonalGCost = 14;

		private Grid<PathNode> _grid;

		private PathNode _startNode;
		private PathNode _destinationNode;

		public PathFinder(Grid<PathNode> grid) => _grid = grid;
		public PathFinder(Grid<PathNode> grid, int normalWalkCost, int diagonalWalkCost)
		{
			_grid = grid;
			_normalGCost = normalWalkCost;
			_diagonalGCost = diagonalWalkCost;
		}

		public Stack<PathNode> GetPath(PathNode startNode, PathNode destinationNode)
		{
			List<PathNode> openList = new List<PathNode>();
			List<PathNode> closedList = new List<PathNode>();

			_startNode = startNode;
			_destinationNode = destinationNode;

			_startNode.ParentNode = null;
			_startNode.gScore = 0;

			openList.Add(_startNode);

			do
			{
				PathNode currentNode = GetNodeWithLowestFScore(openList);

				closedList.Add(currentNode);
				openList.Remove(currentNode);

				if (closedList.Contains(_destinationNode))
				{
					//Debug.Log(closedList.Count + " items in list");
					break;
				}

				foreach (var neighbourNode in GetWalkableAdjacentNodes(currentNode))
				{
					if (closedList.Contains(neighbourNode)) { continue; }

					if (openList.Contains(neighbourNode) == false)
					{
						neighbourNode.ParentNode = currentNode;

						SetGScore(neighbourNode);
						SetHScore(neighbourNode);

						openList.Add(neighbourNode);
					}
					else
					{
						int fScoreBefore = neighbourNode.fScore;

						SetGScore(neighbourNode);
						SetHScore(neighbourNode);

						if (neighbourNode.fScore < fScoreBefore)
						{
							neighbourNode.ParentNode = currentNode;
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
				if (node.fScore < nodeWithLowestScore.fScore)
				{
					nodeWithLowestScore = node;
				}
			}

			return nodeWithLowestScore;
		}
		
		private List<PathNode> GetWalkableAdjacentNodes(PathNode node)
		{
			List<PathNode> adjacentNodes = new List<PathNode>();

			Vector2Int nodePosition = _grid.GetGridPosition(node.Position);

			for (int x = -1; x < 2; x++)
			{
				for (int y = -1; y < 2; y++)
				{
					PathNode potentialNode = _grid.GetCell(nodePosition.x + x, nodePosition.y + y);
					
					if (potentialNode.Walkable) 
					{
						if (Mathf.Abs(x + y) >= 2) { potentialNode.Diagonal = true; } 
						else { potentialNode.Diagonal = false; }

						adjacentNodes.Add(potentialNode); 
					}
				}
			}

			adjacentNodes.Remove(node);

			return adjacentNodes;
		}

		private void SetGScore(PathNode node)
		{
			int parentGScore = 0;

			if (node.ParentNode != null) { parentGScore = node.ParentNode.gScore; }

			if (node.Diagonal) { node.gScore = parentGScore + _diagonalGCost; }
			else { node.gScore = parentGScore + _normalGCost; }
		}
		private void SetHScore(PathNode node) => node.hScore = GetDistanceCost(node, _destinationNode) * 10;
		
		private int GetDistanceCost(PathNode nodeA, PathNode nodeB)
		{
			Vector2Int nodeAPosition = _grid.GetGridPosition(nodeA.Position);
			Vector2Int nodeBPosition = _grid.GetGridPosition(nodeB.Position);
			return Mathf.Abs((nodeBPosition.x - nodeAPosition.x) + (nodeBPosition.y - nodeAPosition.y));
		}

		private Stack<PathNode> ConvertToFinishedPathStack(List<PathNode> pathNodes)
		{
			Stack<PathNode> finishedPath = new Stack<PathNode>();
			pathNodes.Reverse();
			finishedPath.Push(pathNodes[0]);

			PathNode currentNode = pathNodes[0];
			while (currentNode.ParentNode != null)
			{
				finishedPath.Push(currentNode.ParentNode);
				currentNode = currentNode.ParentNode;
			}

			return finishedPath;
		}
	}
}
