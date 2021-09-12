using System.Collections.Generic;

namespace CITools
{
	public class Grid<T>
	{
		public T[,] Cells { get; set; }

		private readonly int _width;
		private readonly int _height;
		
		public T this[int x, int y]
		{
			get => Cells[x, y];
			set => Cells[x, y] = value;
		}
		public T this[GridPosition gridPosition]
		{
			get => Cells[gridPosition.x, gridPosition.y];
			set => Cells[gridPosition.x, gridPosition.y] = value;
		}

		public Grid(int width, int height)
		{
			Cells = new T[width, height];

			_width = width;
			_height = height;
		}

		public T GetAt(int x, int y) => this[x, y];
		public T GetAt(GridPosition gridPosition) => this[gridPosition];

		public void SetAt(int x, int y, T value) => this[x, y] = value;
		public void SetAt(GridPosition gridPosition, T value) => this[gridPosition] = value;

		public List<T> GetAround(int x, int y)
		{
			var nodes = new List<T>();
			for (int xOffset = -1; xOffset <= 1; xOffset++)
			{
				for (int yOffset = -1; yOffset <= 1; yOffset++)
				{
					var currentX = x + xOffset;
					var currentY = y + yOffset;

					if (!IsInsideGrid(currentX, currentY)) continue;

					nodes.Add(Cells[currentX, currentY]);

				}
			}
			nodes.Remove(GetAt(x, y));
			return nodes;
		}
		public List<T> GetAround(GridPosition position) => GetAround(position.x, position.y);

		public bool IsInsideGrid(int x, int y)
		{
			if (x < 0 || x >= _width) return false;
			if (y < 0 || y >= _height) return false;
			return true;
		}
		public bool IsInsideGrid(GridPosition position) => IsInsideGrid(position.x, position.y);
	}
}
