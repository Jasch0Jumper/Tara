using UnityEngine;

namespace Tara.Pathfinding
{
	public class PathNode
	{
		public Vector3 Position { get; private set; }
		public Vector2Int GridPosition { get; private set; }
		public bool Walkable { get; set; }
		public int WalkCost { get; private set; }

		public PathNode ParentNode { get; private set; }

		public int FScore { get => _cachedFScore; }
		public int GScore { get; private set; }
		public int HScore { get; private set; }

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
			
			if (ParentNode != null) { parentGScore = ParentNode.GScore; }

			GScore = parentGScore + WalkCost;

			RefreshFScore();
		}
		private void RefreshFScore() => _cachedFScore = GScore + HScore;
		public void SetHScore(int value)
		{
			HScore = value;
			RefreshFScore();
		}

		public void Reset()
		{
			ParentNode = null;
			GScore = 0;
			HScore = 0;
		}
	}
}
