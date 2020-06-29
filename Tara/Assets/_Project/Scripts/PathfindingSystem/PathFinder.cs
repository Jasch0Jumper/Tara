using System.Collections.Generic;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class PathFinder : MonoBehaviour
	{
		private bool _pathGenerated;

		private WayPointManager _wayPointManager;
		
		private List<WayPoint> _wayPoints = new List<WayPoint>();
		private List<WayPoint> _openWayPoints = new List<WayPoint>();
		private List<WayPoint> _closedWayPoints = new List<WayPoint>();

		private Stack<Vector2> _pathToTarget = new Stack<Vector2>();

		[Header("Gizmos")]
		[SerializeField] private bool drawPathOnDeselect = default;

		private void Awake()
		{
			_wayPointManager = FindObjectOfType<WayPointManager>();
		}

		public void TargetReached()
		{
			_pathGenerated = false;
		}

		public Vector2 PathFindTo(Vector2 target)
		{
			if (_pathGenerated != true) { _pathToTarget = GeneratePath(target, transform.position); }

			return _pathToTarget.Pop();
		}

		private Stack<Vector2> GeneratePath(Vector2 targetPosition, Vector2 startPosition)
		{
			Stack<Vector2> pathPointPositions = new Stack<Vector2>();

			WayPoint targetPoint = _wayPointManager.GetClosestWayPoint(targetPosition);
			WayPoint startPoint = _wayPointManager.GetClosestWayPoint(startPosition);

			WayPoint currentWayPoint = startPoint;

			int i = 0;

			do
			{
				_closedWayPoints.Add(currentWayPoint);

				foreach (var availablePoint in GetAvailableWayPoints(currentWayPoint))
				{
					int currentScore = GetPointScore(currentWayPoint, startPoint, targetPoint);
					int availableScore = GetPointScore(availablePoint, startPoint, targetPoint);

					_openWayPoints.Add(availablePoint);

					if (availableScore < currentScore)
					{
						currentWayPoint = availablePoint;
						_openWayPoints.Remove(availablePoint);
					}
				}

				i++;
			}
			while (i < 50);

			_pathGenerated = true;

			return pathPointPositions;
		}

		private List<WayPoint> GetAvailableWayPoints(WayPoint wayPoint)
		{
			List<WayPoint> availableWayPoints = new List<WayPoint>();
			WayPoint[] neighborWayPoints = _wayPointManager.GetNeighborWayPoints(wayPoint);

			for (int i = 0; i < neighborWayPoints.Length; i++)
			{
				if (neighborWayPoints[i].Active)
				{ availableWayPoints.Add(neighborWayPoints[i]); }
			}

			return availableWayPoints;
		}

		private int GetPointScore(WayPoint wayPoint, WayPoint startPoint, WayPoint targetPoint)
		{
			int unit = Mathf.RoundToInt(_wayPointManager.spaceBetweenPoints);

			int movementCostFromStart = DistanceAsInt(startPoint.Position, wayPoint.Position) / unit;
			int absoluteDistanceToTarget = DistanceAsInt(targetPoint.Position, wayPoint.Position) / unit;

			return movementCostFromStart + absoluteDistanceToTarget;
		}

		private int DistanceAsInt(Vector2 startPosition, Vector2 targetPosition)
		{
			Vector2 distance;
			distance.x = Mathf.Abs(targetPosition.x - startPosition.x);
			distance.y = Mathf.Abs(targetPosition.y - startPosition.y);

			return Mathf.RoundToInt(distance.x + distance.y);
		}

		#region Gizmos

		private void OnDrawGizmos()
		{
			if (drawPathOnDeselect) { DrawPath(); }
		}

		private void OnDrawGizmosSelected()
		{
			DrawPath();
		}

		private void DrawPath()
		{
			if (_pathToTarget.Count != 0)
			{
				foreach (var waypoint in _pathToTarget)
				{
					Gizmos.color = Color.cyan;
					Gizmos.DrawWireSphere(waypoint, 1f);
				}
				for (int i = 0; i < _pathToTarget.Count; i++)
				{
					Vector2[] points = _pathToTarget.ToArray();

					int index = 0;
					if (i + 1 > points.Length) { index = -1; }

					Gizmos.color = Color.cyan;
					Gizmos.DrawLine(points[index], points[index + 1]);

					index++;
				}
			}
		}

		#endregion Gizmos
	}
}