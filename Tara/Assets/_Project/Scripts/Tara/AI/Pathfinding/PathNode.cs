using System.Collections.Generic;
using UnityEngine;

namespace Tara.Pathfinding
{
	public class PathNode
	{
		public GridNode GridNode { get; private set; }
		public GridNode Destination { get; private set; }

		public PathNode ParentNode { get; private set; }

		public int FScore { get => GScore + HScore; }
		public int GScore { get; private set; }
		
		public int HScore { get; private set; }

		public PathNode(GridNode gridNode, GridNode destination)
		{
			GridNode = gridNode;
			Destination = destination;

			GScore = gridNode.Cost;

			SetHScore();
		}
		public PathNode(GridNode gridNode, PathNode parent)
		{
			GridNode = gridNode;
			SetParent(parent);
		}

		public void SetParent(PathNode parent)
		{
			ParentNode = parent;
			Destination = parent.Destination;

			SetGScore();
			SetHScore();
		}

		private void SetGScore()
		{
			if (IsDiagonal(ParentNode.GridNode))
			{
				GScore = Mathf.RoundToInt(GridNode.Cost * 1.5f);
			}
			GScore += ParentNode.GScore;
		}

		private void SetHScore()
		{
			var deltaX = Mathf.Abs(GridNode.GridPosition.x - Destination.GridPosition.x);
			var deltaY = Mathf.Abs(GridNode.GridPosition.y - Destination.GridPosition.y);

			HScore = (deltaX + deltaY) * 100;
		}
		
		private bool IsDiagonal(GridNode node)
		{
			var deltaX = Mathf.Abs(node.GridPosition.x - GridNode.GridPosition.x);
			var deltaY = Mathf.Abs(node.GridPosition.y - GridNode.GridPosition.y);
			return deltaX > 0 && deltaY > 0;
		}

		public static bool operator ==(PathNode left, PathNode right) => EqualityComparer<PathNode>.Default.Equals(left, right);
		public static bool operator !=(PathNode left, PathNode right) => !(left == right);

		public override bool Equals(object obj)
		{
			return obj is PathNode node &&
				   EqualityComparer<GridNode>.Default.Equals(GridNode, node.GridNode) &&
				   EqualityComparer<GridNode>.Default.Equals(Destination, node.Destination);
		}
		public override int GetHashCode()
		{
			int hashCode = -1102133632;
			hashCode = hashCode * -1521134295 + GridNode.GetHashCode();
			hashCode = hashCode * -1521134295 + Destination.GetHashCode();
			return hashCode;
		}
	}
}
