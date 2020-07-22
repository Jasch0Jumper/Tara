using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class PathNode
	{
		public Vector3 Position { get; private set; }
		public bool Walkable { get; set; }

		public PathNode ParentNode { get; set; }

#pragma warning disable IDE1006 // Naming Styles
		public int fScore { get => gScore + hScore; }
		public int gScore { get; set; }
		public int hScore { get; set; }
#pragma warning restore IDE1006 // Naming Styles
		public bool Diagonal { get; set; } = false;

		public PathNode()
		{
			Walkable = true;
		}

		public void SetPosition(Vector3 position) => Position = position;
	}
}
