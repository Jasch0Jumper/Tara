using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class PathNode
	{
		public Vector3 Position { get; private set; }
		public Vector2Int GridPosition { get; private set; }
		public bool Walkable { get; set; }
		public int WalkCost { get; private set; }

		public PathNode ParentNode { get; private set; }

#pragma warning disable IDE1006 // Naming Styles
		public int fScore { get => _cachedFScore; }
		public int gScore { get; private set; }
		public int hScore { get; private set; }
#pragma warning restore IDE1006 // Naming Styles
		private int _cachedFScore;

		public PathNode() { }
		public void Initialize(Vector3 globalPos, Vector2Int gridPos)
		{
			Position = globalPos;
			GridPosition = gridPos;
			Walkable = true;
		}

		public void SetParent(PathNode parent)
		{
			ParentNode = parent;
			RefreshGScore();
		}
		public void SetWalkCost(int cost)
		{
			WalkCost = cost;
			RefreshGScore();
		}

		public void RefreshGScore()
		{
			int parentGScore = 0;
			
			if (ParentNode != null) { parentGScore = ParentNode.gScore; }

			gScore = parentGScore + WalkCost;

			RefreshFScore();
		}
		private void RefreshFScore() => _cachedFScore = gScore + hScore;
		public void SetHScore(int value)
		{
			hScore = value;
			RefreshFScore();
		}

		public void ResetNode()
		{
			ParentNode = null;
			gScore = 0;
			hScore = 0;
		}
	}
}
