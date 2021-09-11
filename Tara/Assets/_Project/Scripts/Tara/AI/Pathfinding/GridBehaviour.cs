using System.Collections.Generic;
using UnityEngine;
using CITools;

namespace Tara.Pathfinding
{
	public class GridBehaviour : MonoBehaviour
	{
		[SerializeField] [Range(1f, 20f)] private float cellSize = 5f;
		[SerializeField] [Range(0, 200)] private int width = default;
		[SerializeField] [Range(0, 200)] private int height = default;
		[SerializeField] private Vector2 origin = default;
		[Space]
		[SerializeField] private int defaultCost = 10;

		private Grid<GridNode> _grid;

		public Grid<GridNode> Grid 
		{ 
			get
			{
				if (_grid is null)
				{
					GenerateGrid();
					return _grid;
				}
				return _grid;
			}
		}

		private void Awake()
		{
			GenerateGrid();
		}

		public GridNode GetNodeAt(int x, int y) => Grid[x, y];
		public GridNode GetNodeAt(Vector3 position)
		{
			var gridPos = GlobalToGrid(position);

			if (IsOutsideGrid(gridPos.x, gridPos.y)) return default;

			return GetNodeAt(gridPos.x, gridPos.y);
		}

		public List<GridNode> GetNodesAround(int x, int y)
		{
			var nodes = new List<GridNode>();
			for (int xOffset = -1; xOffset <= 1; xOffset++)
			{
				for (int yOffset = -1; yOffset <= 1; yOffset++)
				{
					var currentX = x + xOffset;
					var currentY = y + yOffset;

					if (IsOutsideGrid(currentX, currentY)) continue;
					
					nodes.Add(Grid[currentX, currentY]);
				
				}
			}
			nodes.Remove(GetNodeAt(x, y));
			return nodes;
		}
		public List<GridNode> GetNodesAround(GridNode node)
		{
			return GetNodesAround(node.GridPosition.x, node.GridPosition.y);
		}
		public List<GridNode> GetNodesAround(Vector3 position)
		{
			var node = GetNodeAt(position);
			return GetNodesAround(node);
		}

		public void BlockCells(BlockedPoints cells)
		{
			foreach(var point in cells.GetPointsInArea(cellSize))
			{
				var gridPos = GlobalToGrid(point);

				if (IsOutsideGrid(gridPos.x, gridPos.y)) continue;
				
				Grid.Cells[gridPos.x, gridPos.y].Walkable = false;
			}
		}
		public void UnBlockCells(BlockedPoints cells)
		{
			foreach (var point in cells.GetPointsInArea(cellSize))
			{
				var gridPos = GlobalToGrid(point);

				if (IsOutsideGrid(gridPos.x, gridPos.y)) continue;

				Grid.Cells[gridPos.x, gridPos.y].Walkable = true;
			}
		}

		[ContextMenu("Generate Grid")]
		private void GenerateGrid()
		{
			_grid = new Grid<GridNode>(width, height);

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					var position = new Vector3(origin.x + cellSize * x, origin.y + cellSize * y);
					var gridPosition = new Vector2Int(x, y);
					Grid[x, y] = new GridNode(position, gridPosition, defaultCost);
				}
			}
		}

		[ContextMenu("Remove Grid")]
		private void RemoveGrid() => _grid = null;

		private Vector2Int GlobalToGrid(Vector3 position)
		{
			var x = Mathf.RoundToInt((position.x / cellSize) - (origin.x / cellSize));
			var y = Mathf.RoundToInt((position.y / cellSize) - (origin.y / cellSize));
			return new Vector2Int(x, y);
		}

		private bool IsOutsideGrid(int x, int y)
		{
			if (x < 0 || x >= width) return true;
			if (y < 0 || y >= height) return true;
			return false;
		}

		#region Gizmos
#if UNITY_EDITOR
		[Header("Gizmos")]
		[SerializeField] private bool showOutline = default;
		[SerializeField] private bool showGrid = default;
		[SerializeField] private bool showUnwalkable = default;

		private void OnDrawGizmos()
		{
			if (showOutline) DrawOutline();
			if (showGrid) DrawGrid();
			if (showUnwalkable) DrawUnwalkable();
		}
		private void OnDrawGizmosSelected()
		{
			DrawGrid();
			DrawOutline();
			DrawUnwalkable();
		}

		private void DrawOutline()
		{
			float gridWidth = cellSize * width;
			float gridHeight = cellSize * height;

			var offset = new Vector3(origin.x - (cellSize / 2), 0f + origin.y - (cellSize / 2));

			Vector3 bottomLeftCorner = new Vector3(0f, 0f) + offset;
			Vector3 bottomRightCorner = new Vector3(gridWidth , 0f) + offset; 
			Vector3 topLeftCorner = new Vector3(0f, gridHeight) + offset; 
			Vector3 topRightCorner = new Vector3(gridWidth, gridHeight) + offset;

			Gizmos.color = Color.green;

			if (width > 0)
			{
				Gizmos.DrawLine(bottomLeftCorner, bottomRightCorner);	
				Gizmos.DrawLine(topLeftCorner, topRightCorner);
			}
			if (height > 0)
			{
				Gizmos.DrawLine(bottomLeftCorner, topLeftCorner);
				Gizmos.DrawLine(bottomRightCorner, topRightCorner);
			}
		}
		private void DrawGrid()
		{
			if (Grid == null) { return; }

			Gizmos.color = Color.white;

			foreach (var cell in Grid.Cells)
			{
				Gizmos.DrawWireCube(cell.WorldPosition, Vector3.one * cellSize);
			}
		}
		private void DrawUnwalkable()
		{
			if (Grid == null) { return; }

			Gizmos.color = Color.red;

			foreach (var cell in Grid.Cells)
			{
				if (cell.Walkable) continue;

				Gizmos.DrawWireCube(cell.WorldPosition, Vector3.one * cellSize);
			}
		}
#endif
		#endregion
	}
}
