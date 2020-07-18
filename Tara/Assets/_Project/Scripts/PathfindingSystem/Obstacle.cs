using UnityEngine;
using System.Collections.Generic;

namespace Tara.PathfindingSystem 
{
	[ExecuteInEditMode]
	public class Obstacle : MonoBehaviour
	{
		public static List<Obstacle> Obstacles = new List<Obstacle>();

		[SerializeField] private BlockPointChain blockedArea = default;
		[Header("Gizmos")]
		[SerializeField] private bool showPoints = default;
		[SerializeField] private bool showAllPoints = default;
		[SerializeField] private bool showBlockPointsPath = default;

		public delegate void ObstacleEvent(BlockPointChain blockedArea);
		public event ObstacleEvent OnSpawn;
		public event ObstacleEvent OnDespawn;

		private Vector3 _lastPosition;
		
		private void Start()
		{
			MovePoints();
			_lastPosition = transform.position;
		}
		private void OnEnable()
		{
			Obstacles.Add(this);

			OnSpawn?.Invoke(blockedArea);
		}
		private void OnDisable()
		{
			OnDespawn?.Invoke(blockedArea);

			Obstacles.Remove(this);
		}
		private void Update()
		{
			MovePoints();
		}

		private void MovePoints() => blockedArea.MovePoints(transform.position);

		#region Gizmos
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

			foreach (var point in blockedArea.GetPoints())
			{
				Gizmos.DrawWireSphere(point, 1f);
			}
		}
		private void DrawAllPoints()
		{
			Gizmos.color = new Color(1f, 0.921568632f, 0.0156862754f, 0.25f);	//yellow with 50% alpha

			foreach (var point in blockedArea.GetPointsInArea(GridManager.CELLSIZE))
			{
				Gizmos.DrawWireSphere(point, 2.5f);
			}
		}
		private void DrawBlockPointPath()
		{
			Gizmos.color = Color.red;

			List<Vector3> blockedPoints = blockedArea.GetPointsInArea(GridManager.CELLSIZE);

			for (int i = 0; i < blockedPoints.Count - 1; i++)
			{
				Gizmos.DrawLine(blockedPoints[i], blockedPoints[i + 1]);
			}
		}
		#endregion
	}
}
