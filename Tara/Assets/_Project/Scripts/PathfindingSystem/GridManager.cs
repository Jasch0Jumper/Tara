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
		private Vector3 _offsetVector => offset;

		private void Start()
		{
			GenerateGrid();
		}

		[ContextMenu("Generate Grid")]
		private void GenerateGrid() => _grid = new Grid(width, height, cellSize, transform.position + _offsetVector);
		[ContextMenu("Remove Grid")]
		private void RemoveGrid() => _grid = null;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				_grid.ToggleWalkable(Camera.main.ScreenToWorldPoint(Input.mousePosition), false);
			}
		}

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
			if (_grid != null)
			{
				foreach (var cell in _grid.GridArray)
				{
					Gizmos.color = Color.white;
					Gizmos.DrawWireCube(cell.GetGlobalPosition() + new Vector3(cell.Size, cell.Size) / 2, new Vector3(cell.Size, cell.Size, 1f));
				}
				foreach (var cell in _grid.GridArray)
				{
					if (cell.Walkable == false)
					{
						Gizmos.color = Color.red;
						Gizmos.DrawWireCube(cell.GetGlobalPosition() + new Vector3(cell.Size, cell.Size) / 2, new Vector3(cell.Size, cell.Size, 1f));
					}
				}
			}
		}
		#endregion
	}
}
