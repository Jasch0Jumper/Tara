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

		public delegate void ObstacleEvent(BlockPointChain blockedAreas);
		public event ObstacleEvent OnSpawn;
		public event ObstacleEvent OnMove;
		public event ObstacleEvent OnDespawn;

		private Vector3 _lastPosition;
		private Vector3 _deltaPosition;

		private void Start()
		{
			MovePoints(transform.position);
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
			MovePoints(_deltaPosition);
		}

		private void MovePoints(Vector3 deltaPosition)
		{
			
			if (HasMovedOneGridStep())
			{
				blockedArea.Move(Coordinate.Convert(transform.position));

				OnMove?.Invoke(blockedArea);
			}
		}

		private bool HasMovedOneGridStep()
		{
			_deltaPosition.x = Mathf.Abs(_lastPosition.x - transform.position.x);
			_deltaPosition.y = Mathf.Abs(_lastPosition.y - transform.position.y);

			if (_deltaPosition.x >= GridManager.CELLSIZE / 2 || _deltaPosition.y >= GridManager.CELLSIZE / 2)
			{
				_lastPosition = transform.position;
				return true;
			}

			return false;
		}

		#region Gizmos
		private void OnDrawGizmos()
		{
			if (showPoints) { DrawPoints(); }
		}
		private void OnDrawGizmosSelected()
		{
			DrawPoints();
			if (showAllPoints) { DrawAllPoints(); }
		}

		private void DrawPoints()
		{
			Gizmos.color = Color.cyan;

			foreach (var point in blockedArea.Points)
			{
				Gizmos.DrawWireSphere(point.Vector3, 1f);
				;
			}
		}
		private void DrawAllPoints()
		{
			Gizmos.color = Color.yellow;
			foreach (var point in blockedArea.GetPointsInArea(GridManager.CELLSIZE))
			{
				Gizmos.DrawWireSphere(point.Vector3, 2.5f);
			}
		}
		#endregion
	}
}
