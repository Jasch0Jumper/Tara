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
					Debug.Log(closedList.Count + " items in list");
					break;
				}

				List<PathNode> adjacentNodes = GetWalkableAdjacentNodes(currentNode);

				foreach (var node in adjacentNodes)
				{
					if (closedList.Contains(node))
					{
						continue;
					}

					if (openList.Contains(node) == false)
					{
						node.ParentNode = currentNode;

						SetGScore(node);
						SetHScore(node);


						openList.Add(node);
					}
					else
					{
						int fScoreBefore = node.fScore;

						SetGScore(node);
						SetHScore(node);

						if (node.fScore < fScoreBefore)
						{
							node.ParentNode = currentNode;
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

			//adjacentNodes.Add(_grid.GetCell(nodePosition.x + 0, nodePosition.y + 1));
			//adjacentNodes.Add(_grid.GetCell(nodePosition.x + 1, nodePosition.y + 1));
			//adjacentNodes.Add(_grid.GetCell(nodePosition.x + 1, nodePosition.y + 0));
			//adjacentNodes.Add(_grid.GetCell(nodePosition.x + 1, nodePosition.y + -1));
			//adjacentNodes.Add(_grid.GetCell(nodePosition.x + 0, nodePosition.y + -1));
			//adjacentNodes.Add(_grid.GetCell(nodePosition.x + -1, nodePosition.y + -1));
			//adjacentNodes.Add(_grid.GetCell(nodePosition.x + -1, nodePosition.y + 0));
			//adjacentNodes.Add(_grid.GetCell(nodePosition.x + -1, nodePosition.y + 1));
			
			adjacentNodes.Remove(node);

			return adjacentNodes;
		}

		private void SetGScore(PathNode node)
		{
			int parentGScore = node.ParentNode.gScore;
			
			if (node.Diagonal) { node.gScore = parentGScore + _diagonalGCost; }
			else { node.gScore = parentGScore + _normalGCost; }
		}
		private void SetHScore(PathNode node)
		{
			Vector2Int currentNodePosition = _grid.GetGridPosition(node.Position);
			Vector2Int endNodePosition = _grid.GetGridPosition(_destinationNode.Position);
			node.hScore = Mathf.Abs((endNodePosition.x - currentNodePosition.x) + (endNodePosition.y - currentNodePosition.y));
		}

		private Stack<PathNode> ConvertToFinishedPathStack(List<PathNode> pathNodes)
		{
			Stack<PathNode> finishedPath = new Stack<PathNode>();
			
			pathNodes.Reverse();
			finishedPath.Push(pathNodes[0]);

			for (int i = 0; pathNodes[i] != _startNode; i++)
			{
				finishedPath.Push(pathNodes[i].ParentNode);
			}

			return finishedPath;
		}
	}
}
