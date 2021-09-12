using System.Collections.Generic;
using System;
using CITools;

namespace Tara.Pathfinding
{
	public class PathNode
	{
		public GridNode GridNode { get; private set; }

		public PathNode Parent { get; private set; }

		public int FScore { get => GScore + HScore; }
		public int GScore { get => CalculateGScore(); }
		public int HScore { get => CalculateHScore(); }
		
		private GridPosition _destination;

		public PathNode(GridNode gridNode, GridNode destination)
		{
			GridNode = gridNode;
			_destination = destination.Position;
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
				return (int)Math.Round(GridNode.Cost * 1.4f) + Parent.GScore;
			}
			return GridNode.Cost + Parent.GScore;
		}
		private int CalculateHScore()
		{
			var deltaX = Math.Abs(GridNode.Position.x - _destination.x);
			var deltaY = Math.Abs(GridNode.Position.y - _destination.y);

			var notDiagonal = Math.Abs(deltaX - deltaY);

			int cost;

			if (deltaX > deltaY)
				cost = notDiagonal + (deltaX - notDiagonal);
			else
				cost = notDiagonal + (deltaY - notDiagonal);
			
			return cost * GridNode.Cost;
		}
		
		private bool IsDiagonal(GridNode node)
		{
			var deltaX = Math.Abs(GridNode.Position.x - node.Position.x);
			var deltaY = Math.Abs(GridNode.Position.y - node.Position.y);
			
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
