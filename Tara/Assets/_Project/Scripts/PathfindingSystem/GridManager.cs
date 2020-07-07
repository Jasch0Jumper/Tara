using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class GridManager : MonoBehaviour
	{
		[SerializeField] [Range(0, 200)] private int width = default;
		[SerializeField] [Range(0, 200)] private int height = default;
		[SerializeField] private float cellSize = default;
		[SerializeField] private Vector2 offset = default;
		[Header("Gizmos")]
		[SerializeField] private bool showPreview = default;
		[SerializeField] private bool showGrid = default;

		private Grid _grid;
		private Vector3 _offset { get => offset; }

		private void Start()
		{
			GenerateGrid();
		}

		[ContextMenu("GenerateGrid")]
		private void GenerateGrid() => _grid = new Grid(width, height, cellSize, transform.position + _offset);

		#region Gizmos
		private void OnDrawGizmos()
		{
			if (showGrid) { DrawGrid(); }
			if (showPreview) { DrawPreview(); }
		}
		private void OnDrawGizmosSelected()
		{
			DrawGrid();
			DrawPreview();
		}

		private void DrawPreview()
		{
			float gridWidth = cellSize * width;
			float gridHeight = cellSize * height;

			Vector3 bottomLeftCorner = transform.position + new Vector3(0f, 0f) + _offset;
			Vector3 bottomRightCorner = transform.position + new Vector3(gridWidth, 0f) + _offset; 
			Vector3 topLeftCorner = transform.position + new Vector3(0f, gridHeight) + _offset; 
			Vector3 topRightCorner = transform.position + new Vector3(gridWidth, gridHeight) + _offset; 

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
			if (_grid != null)
			{
				foreach (var cell in _grid.GridArray)
				{
					Gizmos.DrawWireCube(cell.GetGlobalPosition() + new Vector3(cell.Size, cell.Size) / 2, new Vector3(cell.Size, cell.Size, 1f));
				}
			}
		}
		#endregion
	}
}
