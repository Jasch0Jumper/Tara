﻿using System.Collections.Generic;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class GridManager : MonoBehaviour
	{
		public const float CELLSIZE = 5f;
		[Header("CELLSIZE = 5f")]
		[SerializeField] [Range(0, 200)] private int width = default;
		[SerializeField] [Range(0, 200)] private int height = default;
		[SerializeField] private Vector2 offset = default;
		[SerializeField] [Range(1f, 10f)] private float gridRefreshTime = default;
		[Header("Gizmos")]
		[SerializeField] private bool showPreview = default;
		[SerializeField] private bool showGrid = default;
		[SerializeField] private bool showUnwalkable = default;

		public static List<BlockPointChain> ObstacleAreas = new List<BlockPointChain>();

		private Grid<PathNode> _grid;
		private Timer _timer;

#pragma warning disable IDE1006 // Naming Styles
		private Vector3 _offsetVector => offset;
#pragma warning restore IDE1006 // Naming Styles

		private void Awake()
		{
			GenerateGrid();

			_timer = new Timer(gridRefreshTime, true);
			_timer.OnTimerEnd += RefreshGrid;
		}
		private void Update()
		{
			_timer.Tick(Time.deltaTime);
		}

		[ContextMenu("Generate Grid")]
		private void GenerateGrid() => _grid = new Grid<PathNode>(width, height, CELLSIZE, transform.position + _offsetVector, () => new PathNode(true));

		[ContextMenu("Remove Grid")]
		private void RemoveGrid() => _grid = null;

		private void RefreshGrid()
		{
			foreach (var cell in _grid.Cells)
			{
				cell.Walkable = true;
			}

			ToggleWalkableArea(ObstacleAreas, false);
		}

		private void ToggleWalkableArea(List<BlockPointChain> areas, bool state)
		{
			foreach (var area in areas)
			{
				foreach (var point in area.GetPointsInArea(CELLSIZE))
				{
					_grid.GetCell(point).Walkable = state;
				}
			}
		}

		#region Gizmos
		private void OnDrawGizmos()
		{
			if (showGrid) { DrawGrid(); }
			if (showPreview) { DrawPreview(); }
			if (showUnwalkable) { DrawUnwalkable(); }
		}
		private void OnDrawGizmosSelected()
		{
			DrawGrid();
			DrawPreview();
			DrawUnwalkable();
		}

		private void DrawPreview()
		{
			float gridWidth = CELLSIZE * width;
			float gridHeight = CELLSIZE * height;

			Vector3 bottomLeftCorner = transform.position + new Vector3(0f, 0f) + _offsetVector;
			Vector3 bottomRightCorner = transform.position + new Vector3(gridWidth, 0f) + _offsetVector; 
			Vector3 topLeftCorner = transform.position + new Vector3(0f, gridHeight) + _offsetVector; 
			Vector3 topRightCorner = transform.position + new Vector3(gridWidth, gridHeight) + _offsetVector;

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
			if (_grid == null) { return; }

			Gizmos.color = Color.white;

			_grid.ForeachCell(delegate (PathNode cell, Vector3 cellGloablPosition)
			{
				Gizmos.DrawWireCube(cellGloablPosition + new Vector3(_grid.CellSize, _grid.CellSize) / 2, new Vector3(_grid.CellSize, _grid.CellSize, 1f));
			});
		}
		private void DrawUnwalkable()
		{
			if (_grid == null) { return; }

			Gizmos.color = Color.red;

			_grid.ForeachCell(delegate (PathNode cell, Vector3 cellGloablPosition)
			{
				if (cell.Walkable == false)
				{
					Gizmos.DrawWireCube(cellGloablPosition + new Vector3(_grid.CellSize, _grid.CellSize) / 2, new Vector3(_grid.CellSize, _grid.CellSize, 1f));
				}
			});
		}
		#endregion
	}
}
