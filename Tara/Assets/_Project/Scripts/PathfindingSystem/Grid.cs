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
		
		public TGridObject GetCell(int x, int y) => Cells[x, y];
		public TGridObject GetCell(Vector3 position)
		{
			Vector2Int gridPosition = GetGridPosition(position);
			int x = gridPosition.x;
			int y = gridPosition.y;
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
	}
}
