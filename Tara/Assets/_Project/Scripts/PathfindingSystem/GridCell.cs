using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class GridCell
	{
		public int Data { get; set; }

		public Vector2Int Position { get; }
		
		private float _size;

		public GridCell(Vector2Int position, float size)
		{
			Position = position;
			_size = size;

			Debug.Log($"Grid Cell: grid position:{Position}, global position: {GetGlobalPosition()}, data: {Data}");
		}

		public Vector3 GetGlobalPosition() => new Vector3(Position.x, Position.y) * _size;
	}
}

