using System;
using System.Collections.Generic;
using UnityEngine;

namespace CITools
{
	public class Grid<TGridItem>
	{
		private TGridItem[,] _items;
		public float CellSize { get; private set; }

		private int _width;
		private int _height;
		private Vector3 _originPosition;

		public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<TGridItem> objectInitialization)
		{
			CellSize = cellSize;
			_width = width;
			_height = height;
			_originPosition = originPosition;

			_items = new TGridItem[_width, _height];
			ForeachItem(objectInitialization);
		}

		public Vector3 GetGlobalPosition(int x, int y) => new Vector3(x, y) * CellSize + _originPosition;
		public Vector2Int GetGridPosition(Vector3 position)
		{
			int x = (int)Math.Round((position.x - _originPosition.x) / CellSize);
			int y = (int)Math.Round((position.y - _originPosition.y) / CellSize);

			return new Vector2Int(x, y);
		}

		public TGridItem GetCellAt(int x, int y) => _items[ValidateX(x), ValidateY(y)];
		public TGridItem GetCellAt(Vector3 position)
		{
			Vector2Int gridPosition = GetGridPosition(position);

			int x = ValidateX(gridPosition.x);
			int y = ValidateY(gridPosition.y);

			return _items[x, y];
		}

		public List<TGridItem> GetItemsInRadius(Vector2Int center, int radius)
		{
			List<TGridItem> adjacentNodes = new List<TGridItem>();

			radius = Math.Abs(radius);

			int start = -radius;
			int end = radius + 1;

			for (int x = start; x < end; x++)
			{
				for (int y = start; y < end; y++)
				{
					adjacentNodes.Add(_items[ValidateX(x + center.x), ValidateY(y + center.y)]);
				}
			}

			adjacentNodes.Remove(_items[center.x, center.y]);

			return adjacentNodes;
		}

		public void ForeachItem(Action<TGridItem> action)
		{
			for (int x = 0; x < _items.GetLength(0); x++)
			{
				for (int y = 0; y < _items.GetLength(1); y++)
				{
					action(_items[x, y]);
				}
			}
		}
		public void ForeachItem(Action<TGridItem, Vector3> action)
		{
			for (int x = 0; x < _items.GetLength(0); x++)
			{
				for (int y = 0; y < _items.GetLength(1); y++)
				{
					action(_items[x, y], GetGlobalPosition(x, y));
				}
			}
		}
		public void ForeachItem(Action<TGridItem, Vector2Int> action)
		{
			for (int x = 0; x < _items.GetLength(0); x++)
			{
				for (int y = 0; y < _items.GetLength(1); y++)
				{
					action(_items[x, y], new Vector2Int(x, y));
				}
			}
		}
		public void ForeachItem(Func<TGridItem> func)
		{
			for (int x = 0; x < _items.GetLength(0); x++)
			{
				for (int y = 0; y < _items.GetLength(1); y++)
				{
					_items[x, y] = func();
				}
			}
		}

		private int Validate(int value, int max)
		{
			if (value >= max) { return max - 1; }
			if (value < 0) { return 0; }
			return value;
		}
		private int ValidateX(int value) => Validate(value, _items.GetLength(0));
		private int ValidateY(int value) => Validate(value, _items.GetLength(1));
	}
}
