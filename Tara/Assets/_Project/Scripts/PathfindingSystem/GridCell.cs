using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class GridCell
	{
		public int Data { get; set; }
		public Vector2Int Position { get; private set; }
		public float Size { get; private set; }
		public bool Walkable { get; set; } = true;

		private Vector3 _originPosition;

		public GridCell(Vector2Int position, float size, Vector3 originPosition)
		{
			Position = position;
			Size = size;
			_originPosition = originPosition;
		}

		public Vector3 GetGlobalPosition() => (new Vector3(Position.x, Position.y) * Size) + _originPosition;
	}
}
