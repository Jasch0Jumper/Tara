using System;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class Grid<TGridObject>
	{
		private TGridObject[,] _cells;
		public float CellSize { get; private set; }

		private int _width;
		private int _height;
		private Vector3 _originPosition;

		public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<TGridObject> objectInitialization)
		{
			CellSize = cellSize;
			_width = width;
			_height = height;
			_originPosition = originPosition;

			_cells = new TGridObject[_width, _height];
			ForeachCell(objectInitialization);
		}

		public Vector3 GetGlobalPosition(int x, int y) => (new Vector3(x, y) * CellSize) + _originPosition;
		public Vector2Int GetGridPosition(Vector3 position)
		{
			int x = Mathf.FloorToInt((position.x - _originPosition.x) / CellSize);
			int y = Mathf.FloorToInt((position.y - _originPosition.y) / CellSize);

			return new Vector2Int(x, y);
		}

		public TGridObject GetCell(int x, int y) => _cells[Validate(x, _cells.GetLength(0)), Validate(y, _cells.GetLength(1))];
		public TGridObject GetCell(Vector3 position)
		{
			Vector2Int gridPosition = GetGridPosition(position);

			int x = Validate(gridPosition.x, _cells.GetLength(0));
			int y = Validate(gridPosition.y, _cells.GetLength(1));

			return _cells[x, y];
		}

		public void ForeachCell(Action<TGridObject> action)
		{
			for (int x = 0; x < _cells.GetLength(0); x++)
			{
				for (int y = 0; y < _cells.GetLength(1); y++)
				{
					action(_cells[x, y]);
				}
			}
		}
		public void ForeachCell(Action<TGridObject, Vector3> action)
		{
			for (int x = 0; x < _cells.GetLength(0); x++)
			{
				for (int y = 0; y < _cells.GetLength(1); y++)
				{
					action(_cells[x, y], GetGlobalPosition(x, y));
				}
			}
		}
		public void ForeachCell(Action<TGridObject, Vector2Int> action)
		{
			for (int x = 0; x < _cells.GetLength(0); x++)
			{
				for (int y = 0; y < _cells.GetLength(1); y++)
				{
					action(_cells[x, y], new Vector2Int(x, y));
				}
			}
		}
		public void ForeachCell(Func<TGridObject> func)
		{
			for (int x = 0; x < _cells.GetLength(0); x++)
			{
				for (int y = 0; y < _cells.GetLength(1); y++)
				{
					_cells[x, y] = func();
				}
			}
		}

		private int Validate(int value, int max)
		{
			if (value >= max) { return max - 1; }
			if (value < 0) { return 0; }
			return value;
		}
	}
}
