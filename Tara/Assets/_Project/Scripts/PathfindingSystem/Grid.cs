using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class Grid
	{
		public GridCell[,] GridArray { get; private set; }
		private int _width;
		private int _height;
		private float _cellSize;
		private Vector3 _originPosition;


		public Grid(int width, int height, float cellSize, Vector3 originPosition)
		{
			_width = width;
			_height = height;
			_cellSize = cellSize;
			_originPosition = originPosition;

			GridArray = new GridCell[_width, _height];

			for (int x = 0; x < GridArray.GetLength(0); x++)
			{
				for (int y = 0; y < GridArray.GetLength(1); y++)
				{
					GridArray[x, y] = new GridCell(new Vector2Int(x, y), _cellSize, _originPosition);
				}
			}
		}

		public void ChangeValue(int x, int y)
		{
			GridArray[x, y].Data++;
		}
	}
}
