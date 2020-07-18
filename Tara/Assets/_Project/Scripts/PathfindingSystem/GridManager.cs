using System.Collections.Generic;
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

		private Grid _grid;
		private Timer _timer;
		private List<BlockPointChain> _obstacleAreas = new List<BlockPointChain>();

#pragma warning disable IDE1006 // Naming Styles
		private Vector3 _offsetVector => offset;
#pragma warning restore IDE1006 // Naming Styles

		private void Awake()
		{
			GenerateGrid();

			_timer = new Timer(gridRefreshTime, true);
			_timer.OnTimerEnd += RefreshGrid;
		}
		private void OnEnable()
		{
			foreach (var obstacle in Obstacle.Obstacles)
			{
				obstacle.OnSpawn += BlockGridArea;
				obstacle.OnDespawn += UnBlockGridArea;
			}
		}
		private void OnDisable()
		{
			foreach (var obstacle in Obstacle.Obstacles)
			{
				obstacle.OnSpawn -= BlockGridArea;
				obstacle.OnDespawn -= UnBlockGridArea;
			}
		}
		private void Update()
		{
			_timer.Tick(Time.deltaTime);
		}

		[ContextMenu("Generate Grid")]
		private void GenerateGrid() => _grid = new Grid(width, height, CELLSIZE, transform.position + _offsetVector);
		[ContextMenu("Remove Grid")]
		private void RemoveGrid() => _grid = null;

		private void BlockGridArea(BlockPointChain blockedArea)
		{
			_obstacleAreas.Add(blockedArea);
			ToggleWalkableArea(blockedArea, false);
		}

		private void UnBlockGridArea(BlockPointChain blockedArea)
		{
			_obstacleAreas.Remove(blockedArea);
			ToggleWalkableArea(blockedArea, true);
		}

		private void RefreshGrid()
		{
			foreach (var cell in _grid.GridArray)
			{
				cell.Walkable = true;
			}

			ToggleWalkableArea(_obstacleAreas, false);
		}

		private void ToggleWalkableArea(BlockPointChain area, bool state)
		{
			foreach (var point in area.GetPointsInArea(CELLSIZE))
			{
				_grid.ToggleWalkable(point, state);
			}
		}
		private void ToggleWalkableArea(List<BlockPointChain> areas, bool state)
		{
			foreach (var area in areas)
			{
				ToggleWalkableArea(area, state);
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
			if (_grid == null) return;
			
			foreach (var cell in _grid.GridArray)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawWireCube(cell.GetGlobalPosition() + new Vector3(cell.Size, cell.Size) / 2, new Vector3(cell.Size, cell.Size, 1f));
			}
		}
		private void DrawUnwalkable()
		{
			if (_grid == null) return;

			foreach (var cell in _grid.GridArray)
			{
				if (cell.Walkable == false)
				{
					Gizmos.color = Color.red;
					Gizmos.DrawWireCube(cell.GetGlobalPosition() + new Vector3(cell.Size, cell.Size) / 2, new Vector3(cell.Size, cell.Size, 1f));
				}
			}
		}
		#endregion
	}
}
