using System.Collections.Generic;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class PathFinder
	{
		private int _normalWalkCost = 10;
		private int _diagonalWalkCost = 14;

		private Grid<PathNode> _grid;

		private PathNode _startNode;
		private PathNode _destinationNode;

		public PathFinder(Grid<PathNode> grid) => _grid = grid;
		public PathFinder(Grid<PathNode> grid, int normalWalkCost, int diagonalWalkCost)
		{
			_grid = grid;
			_normalWalkCost = normalWalkCost;
			_diagonalWalkCost = diagonalWalkCost;
		}

		public Stack<PathNode> GetPath(PathNode startNode, PathNode destinationNode)
		{
			List<PathNode> openList = new List<PathNode>();
			List<PathNode> closedList = new List<PathNode>();

			_startNode = startNode;
			_destinationNode = destinationNode;

			_startNode.ResetNode();

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

				List<PathNode> neighbourNodes = GetWalkableNeighbourNodes(currentNode);
				SetGScoresAroundNode(currentNode, neighbourNodes);

				foreach (var neighbourNode in neighbourNodes)
				{
					if (closedList.Contains(neighbourNode)) { continue; }

					if (openList.Contains(neighbourNode) == false)
					{
						neighbourNode.SetParent(currentNode);

						SetHScore(neighbourNode);

						openList.Add(neighbourNode);
					}
					else
					{
						int fScoreBefore = neighbourNode.fScore;

						neighbourNode.RefreshGScore();
						SetHScore(neighbourNode);

						if (neighbourNode.fScore < fScoreBefore)
						{
							neighbourNode.SetParent(currentNode);
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
		
		private List<PathNode> GetWalkableNeighbourNodes(PathNode centerNode)
		{
			List<PathNode> adjacentNodes = new List<PathNode>(_grid.GetItemsInRadius(_grid.GetGridPosition(centerNode.Position), 1));

			for (int i = adjacentNodes.Count - 1; i >= 0; i--)
			{
				PathNode node = adjacentNodes[i];
				if (node.Walkable == false)
				{
					adjacentNodes.Remove(node);
				}
			}

			return adjacentNodes;
		}
		private void SetGScoresAroundNode(PathNode centerNode, List<PathNode> nodes)
		{
			foreach (var node in nodes)
			{
				int deltaX = Mathf.Abs(centerNode.GridPosition.x - node.GridPosition.x);
				int deltaY = Mathf.Abs(centerNode.GridPosition.y - node.GridPosition.y);

				node.SetWalkCost(_normalWalkCost);

				if (deltaX > 1 || deltaY > 1)
				{
					node.SetWalkCost(_diagonalWalkCost);
				}
			}
		}

		private void SetHScore(PathNode node) => node.SetHScore(GetDistanceCost(node, _destinationNode) * 10);
		
		private int GetDistanceCost(PathNode nodeA, PathNode nodeB)
		{
			Vector2Int nodeAPosition = _grid.GetGridPosition(nodeA.Position);
			Vector2Int nodeBPosition = _grid.GetGridPosition(nodeB.Position);

			return Mathf.RoundToInt(Vector2Int.Distance(nodeAPosition, nodeBPosition));
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
