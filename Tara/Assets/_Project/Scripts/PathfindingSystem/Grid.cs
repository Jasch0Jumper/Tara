using System;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class Grid<TGridObject>
	{
		public TGridObject[,] Cells { get; private set; }
		public float CellSize { get; private set; }

		private int _width;
		private int _height;
		private Vector3 _originPosition;

		public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<TGridObject> objectInitialization)
		{
			_width = width;
			_height = height;
			CellSize = cellSize;
			_originPosition = originPosition;

			Cells = new TGridObject[_width, _height];
			ForeachCell(objectInitialization);
		}

		public Vector3 GetGlobalPosition(int x, int y) => (new Vector3(x, y) * CellSize) + _originPosition;
		
		public TGridObject GetCell(int x, int y) => Cells[Validate(x, Cells.GetLength(0), 0), Validate(y, Cells.GetLength(1), 0)];
		public TGridObject GetCell(Vector3 position)
		{
			Vector2Int gridPosition = GetGridPosition(position);

			int x = Validate(gridPosition.x, Cells.GetLength(0), 0);
			int y = Validate(gridPosition.y, Cells.GetLength(1), 0);

			return Cells[x, y];
		}

		public void ForeachCell(Action<TGridObject> action)
		{
			for (int x = 0; x < Cells.GetLength(0); x++)
			{
				for (int y = 0; y < Cells.GetLength(1); y++)
				{
					action(Cells[x, y]);
				}
			}
		}
		public void ForeachCell(Action<TGridObject, Vector3> action)
		{
			for (int x = 0; x < Cells.GetLength(0); x++)
			{
				for (int y = 0; y < Cells.GetLength(1); y++)
				{
					action(Cells[x, y], GetGlobalPosition(x, y));
				}
			}
		}
		public void ForeachCell(Func<TGridObject> func)
		{
			for (int x = 0; x < Cells.GetLength(0); x++)
			{
				for (int y = 0; y < Cells.GetLength(1); y++)
				{
					Cells[x, y] = func();
				}
			}
		}

		private Vector2Int GetGridPosition(Vector3 position)
		{
			int x = Mathf.FloorToInt((position.x - _originPosition.x) / CellSize);
			int y = Mathf.FloorToInt((position.y - _originPosition.y) / CellSize);

			return new Vector2Int(x, y);
		}

		private int Validate(int value, int max, int min)
		{
			if (value >= max) { return max - 1; }
			if (value < min) { return min; }
			return value;
		}
	}
}
