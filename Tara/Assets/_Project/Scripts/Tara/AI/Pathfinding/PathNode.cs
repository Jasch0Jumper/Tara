using System.Collections.Generic;
using UnityEngine;

namespace Tara.Pathfinding
{
	public class PathNode
	{
		public GridNode GridNode { get; private set; }

		public PathNode Parent { get; private set; }

		public int FScore { get => GScore + HScore; }
		public int GScore { get => CalculateGScore(); }
		public int HScore { get => CalculateHScore(); }
		
		private Vector2Int _destination;

		public PathNode(GridNode gridNode, GridNode destination)
		{
			GridNode = gridNode;
			_destination = destination.GridPosition;
		}

		public PathNode(GridNode gridNode, PathNode parent)
		{
			GridNode = gridNode;
			
			SetParent(parent);
		}

		public void SetParent(PathNode parent)
		{
			Parent = parent;
			_destination = parent._destination;
		}

		private int CalculateGScore()
		{
			if (Parent is null) return 0;

			if (IsDiagonal(Parent.GridNode))
			{
				return Mathf.RoundToInt(GridNode.Cost * 1.4f) + Parent.GScore;
			}
			return GridNode.Cost + Parent.GScore;
		}
		private int CalculateHScore()
		{
			var deltaX = Mathf.Abs(GridNode.GridPosition.x - _destination.x);
			var deltaY = Mathf.Abs(GridNode.GridPosition.y - _destination.y);

			var notDiagonal = Mathf.Abs(deltaX - deltaY);

			int cost;

			if (deltaX > deltaY)
				cost = notDiagonal + (deltaX - notDiagonal);
			else
				cost = notDiagonal + (deltaY - notDiagonal);
			
			return cost * GridNode.Cost;
		}
		
		private bool IsDiagonal(GridNode node)
		{
			var deltaX = Mathf.Abs(GridNode.GridPosition.x - node.GridPosition.x);
			var deltaY = Mathf.Abs(GridNode.GridPosition.y - node.GridPosition.y);
			
			return deltaX > 0 && deltaY > 0;
		}

		public override bool Equals(object obj)
		{
			return obj is PathNode node &&
				   EqualityComparer<GridNode>.Default.Equals(GridNode, node.GridNode) &&
				   _destination.Equals(node._destination);
		}
		public override int GetHashCode()
		{
			int hashCode = 49017651;
			hashCode = hashCode * -1521134295 + GridNode.GetHashCode();
			hashCode = hashCode * -1521134295 + _destination.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(PathNode left, PathNode right)
		{
			return EqualityComparer<PathNode>.Default.Equals(left, right);
		}
		public static bool operator !=(PathNode left, PathNode right)
		{
			return !(left == right);
		}
	}
}
