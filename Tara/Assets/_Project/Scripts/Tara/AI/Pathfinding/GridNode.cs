using UnityEngine;

namespace Tara.Pathfinding
{
	public class GridNode
	{
		public Vector3 WorldPosition { get; private set; }
		public Vector2Int GridPosition { get; private set; }

		public int Cost { get; private set; }

		public bool Walkable { get; set; }

		public GridNode(Vector3 worldPosition, Vector2Int gridPosition, int cost)
		{
			WorldPosition = worldPosition;
			GridPosition = gridPosition;
			Cost = cost;
			Walkable = true;
		}
	}
}
