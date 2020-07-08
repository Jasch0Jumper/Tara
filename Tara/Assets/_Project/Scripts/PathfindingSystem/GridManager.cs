using System.Collections.Generic;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class GridManager : MonoBehaviour
	{
		
		[SerializeField] [Range(0, 200)] private int width = default;
		[SerializeField] [Range(0, 200)] private int height = default;
		[SerializeField] private Vector2 offset = default;
		[SerializeField] private float cellSize = default;
		public float CellSize => cellSize;
		[Header("Gizmos")]
		[SerializeField] private bool showPreview = default;
		[SerializeField] private bool showGrid = default;
		[SerializeField] private bool showUnwalkable = default;

		private Grid _grid;
#pragma warning disable IDE1006 // Naming Styles
		private Vector3 _offsetVector => offset;
#pragma warning restore IDE1006 // Naming Styles

		private void Awake()
		{
			GenerateGrid();
		}

		[ContextMenu("Generate Grid")]
		private void GenerateGrid() => _grid = new Grid(width, height, CellSize, transform.position + _offsetVector);
		[ContextMenu("Remove Grid")]
		private void RemoveGrid() => _grid = null;

		public void BlockGridArea(List<Area> blockedAreas)
		{
			foreach (var area in blockedAreas)
			{
				foreach (var blockedPoint in area.GetPointsInArea(CellSize))
				{
					_grid.ToggleWalkable(blockedPoint, false);
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
			float gridWidth = CellSize * width;
			float gridHeight = CellSize * height;

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
