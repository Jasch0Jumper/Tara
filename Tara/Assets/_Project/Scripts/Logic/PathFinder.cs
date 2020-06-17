using System.Collections.Generic;
using UnityEngine;

namespace Tara
{
	public class PathFinder : MonoBehaviour
	{
		private bool pathGenerated;

		private WayPointManager wayPointManager;
		
		private List<WayPoint> wayPoints = new List<WayPoint>();
		private List<WayPoint> openWayPoints = new List<WayPoint>();
		private List<WayPoint> closedWayPoints = new List<WayPoint>();

		private Stack<Vector2> pathToTarget = new Stack<Vector2>();

		[Header("Gizmos")]
		[SerializeField] private bool drawPathOnDeselect = default;

		private void Awake()
		{
			wayPointManager = FindObjectOfType<WayPointManager>();
		}

		public void TargetReached()
		{
			pathGenerated = false;
		}

		public Vector2 PathFindTo(Vector2 target)
		{
			if (pathGenerated != true) { pathToTarget = GeneratePath(target, transform.position); }

			return pathToTarget.Pop();
		}

		private Stack<Vector2> GeneratePath(Vector2 targetPosition, Vector2 startPosition)
		{
			Stack<Vector2> pathPointPositions = new Stack<Vector2>();

			WayPoint targetPoint = wayPointManager.GetClosestWayPoint(targetPosition);
			WayPoint startPoint = wayPointManager.GetClosestWayPoint(startPosition);

			WayPoint currentWayPoint = startPoint;

			int i = 0;

			do
			{
				closedWayPoints.Add(currentWayPoint);

				foreach (var availablePoint in GetAvailableWayPoints(currentWayPoint))
				{
					int currentScore = GetPointScore(currentWayPoint, startPoint, targetPoint);
					int availableScore = GetPointScore(availablePoint, startPoint, targetPoint);

					openWayPoints.Add(availablePoint);

					if (availableScore < currentScore)
					{
						currentWayPoint = availablePoint;
						openWayPoints.Remove(availablePoint);
					}
				}

				i++;
			}
			while (i < 50);

			pathGenerated = true;

			return pathPointPositions;
		}

		private List<WayPoint> GetAvailableWayPoints(WayPoint wayPoint)
		{
			List<WayPoint> availableWayPoints = new List<WayPoint>();
			WayPoint[] neighborWayPoints = wayPointManager.GetNeighborWayPoints(wayPoint);

			for (int i = 0; i < neighborWayPoints.Length; i++)
			{
				if (neighborWayPoints[i].Active)
				{ availableWayPoints.Add(neighborWayPoints[i]); }
			}

			return availableWayPoints;
		}

		private int GetPointScore(WayPoint wayPoint, WayPoint startPoint, WayPoint targetPoint)
		{
			int unit = Mathf.RoundToInt(wayPointManager.spaceBetweenPoints);

			int movementCostFromStart = DistanceAsInt(startPoint.position, wayPoint.position) / unit;
			int absoluteDistanceToTarget = DistanceAsInt(targetPoint.position, wayPoint.position) / unit;

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
			if (pathToTarget.Count != 0)
			{
				foreach (var waypoint in pathToTarget)
				{
					Gizmos.color = Color.cyan;
					Gizmos.DrawWireSphere(waypoint, 1f);
				}
				for (int i = 0; i < pathToTarget.Count; i++)
				{
					Vector2[] points = pathToTarget.ToArray();

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