using UnityEngine;
using CI.Pathfinding;

namespace Tara.Pathfinding
{
	public class Obstacle : MonoBehaviour
	{
		[SerializeField] private BlockedPoints blockedArea;

		private Vector3 _previousPosition;

		private GridBehaviour _grid;

		private void Awake()
		{
			_grid = FindObjectOfType<GridBehaviour>();
		}
		private void OnEnable()
		{
			Move();
			_grid.Block(blockedArea);
		}
		private void OnDisable()
		{
			_grid.UnBlock(blockedArea);
		}
		private void Update()
		{
			if (_previousPosition == transform.position) return;

			_grid.UnBlock(blockedArea);

			Move();

			_previousPosition = transform.position;

			_grid.Block(blockedArea);
		}

		private void Move() => blockedArea.Move(transform.position);

		#region Gizmos
#if UNITY_EDITOR

		[Header("Gizmos")]
		[SerializeField] private bool showPoints = default;
		[SerializeField] private bool showAllPoints = default;
		[SerializeField] private bool showBlockPointsPath = default;
		
		private void OnDrawGizmos()
		{
			if (showPoints) { DrawPoints(); }
			if (showBlockPointsPath) { DrawBlockPointPath(); }
		}
		private void OnDrawGizmosSelected()
		{
			DrawPoints();
			if (showAllPoints) { DrawAllPoints(); }
		}

		private void DrawPoints()
		{
			Gizmos.color = Color.cyan;

			foreach (var point in blockedArea.GetPointsWithOffset())
			{
				Gizmos.DrawWireSphere(point, 1f);
			}
		}
		private void DrawAllPoints()
		{
			Debug.LogWarning("Gizmo not implemented.", this);

			//Gizmos.color = new Color(1f, 0.921568632f, 0.0156862754f, 0.25f);   //yellow with 50% alpha

			//foreach (var point in blockedArea.GetPointsInArea(GridManager.cellSize))
			//{
			//	Gizmos.DrawWireSphere(point, GridManager.cellSize / 2);
			//}
		}
		private void DrawBlockPointPath()
		{
			Debug.LogWarning("Gizmo not implemented.", this);

			//Gizmos.color = Color.red;

			//List<Vector3> blockedPoints = blockedArea.GetPointsInArea(GridManager.cellSize);

			//for (int i = 0; i < blockedPoints.Count - 1; i++)
			//{
			//	Gizmos.DrawLine(blockedPoints[i], blockedPoints[i + 1]);
			//}
		}
#endif
		#endregion
	}
}
