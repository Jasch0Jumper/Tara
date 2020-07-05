using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class Grid
	{
		private GridCell[,] _gridArray;
		private int _width;
		private int _height;
		private float _cellSize;


		public Grid(int widht, int height, float cellSize)
		{
			_width = widht;
			_height = height;
			_cellSize = cellSize;

			_gridArray = new GridCell[_width, _height];

			for (int x = 0; x < _gridArray.GetLength(0); x++)
			{
				for (int y = 0; y < _gridArray.GetLength(1); y++)
				{
					_gridArray[x, y] = new GridCell(new Vector2Int(x, y), _cellSize);
				}
			}
		}
	}
}
