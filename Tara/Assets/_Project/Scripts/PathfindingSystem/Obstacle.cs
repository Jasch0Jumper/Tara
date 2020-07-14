using UnityEngine;
using System.Collections.Generic;

namespace Tara.PathfindingSystem 
{
	[ExecuteInEditMode]
	public class Obstacle : MonoBehaviour
	{
		[SerializeField] private BlockPointChain blockedArea;
		[Header("Gizmos")]
		[SerializeField] private bool showPoints = default;
		[SerializeField] private bool showAllPoints = default;
		[SerializeField] private bool showBlockPointsPath = default;

		public delegate void ObstacleEvent(BlockPointChain blockedArea);
		public event ObstacleEvent OnSpawn;
		public event ObstacleEvent OnMove;
		public event ObstacleEvent OnDespawn;

		private Vector3 _lastPosition;
#pragma warning disable IDE1006 // Naming Styles
		private Vector3 _deltaPosition => _lastPosition - transform.position;
		private Vector3 debugDeltaPosition;
#pragma warning restore IDE1006 // Naming Styles
		private Vector3 _absoluteDeltaPosition;

		private void Start()
		{
			MovePoints();
			_lastPosition = transform.position;
		}
		private void OnEnable()
		{
			OnSpawn?.Invoke(blockedArea);
		}
		private void OnDisable()
		{
			OnDespawn?.Invoke(blockedArea);
		}
		private void Update()
		{
			debugDeltaPosition = _deltaPosition;

			_absoluteDeltaPosition += new Vector3(Mathf.Abs(_deltaPosition.x), Mathf.Abs(_deltaPosition.y));

			MovePoints();
		}

		private void MovePoints()
		{
			blockedArea.MovePoints(transform.position);
			
			if (HasMovedOneGridStep())
			{
				Debug.Log("OnMove()", this);
				OnMove?.Invoke(blockedArea);
			}
		}

		private bool HasMovedOneGridStep()
		{
			if (_absoluteDeltaPosition.x >= GridManager.CELLSIZE || _absoluteDeltaPosition.y >= GridManager.CELLSIZE)
			{
				_lastPosition = transform.position;
				_absoluteDeltaPosition = Vector3.zero;

				return true;
			}

			return false;
		}

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
