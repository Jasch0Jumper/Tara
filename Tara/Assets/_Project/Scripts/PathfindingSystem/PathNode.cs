namespace Tara.PathfindingSystem
{
	public class PathNode
	{
		public bool Walkable { get; set; }

		public PathNode(bool walkable)
		{
			Walkable = walkable;
		}
	}
}
