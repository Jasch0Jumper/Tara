using CITools;

namespace Tara.Pathfinding
{
	public class GridNode
	{
		public GridPosition Position { get; private set; }

		public int Cost { get; private set; }

		public bool Walkable { get; set; }

		public GridNode(GridPosition gridPosition, int cost)
		{
			Position = gridPosition;
			Cost = cost;
			Walkable = true;
		}
	}
}
