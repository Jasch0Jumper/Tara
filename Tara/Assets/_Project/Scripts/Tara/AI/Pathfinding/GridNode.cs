using UnityEngine;

namespace Tara.Pathfinding
{
	public struct GridNode
	{
		public Vector3 Position { get; private set; }
		public Vector2Int GridPosition { get; private set; }

		public int Cost { get; private set; }

		public bool Walkable { get; set; }

		public GridNode(Vector3 position, Vector2Int gridPosition, int cost)
		{
			Position = position;
			GridPosition = gridPosition;
			Cost = cost;
			Walkable = true;
		}
	}
}
