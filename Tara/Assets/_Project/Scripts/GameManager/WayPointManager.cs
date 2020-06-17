using System.Collections.Generic;
using UnityEngine;

namespace Tara
{
	public class WayPointManager : MonoBehaviour
	{
		[SerializeField] private GameObject wayPoint = default;

		[Header("WayPoint Zone")]
		[SerializeField] private float levelHeight = 1f;

		[SerializeField] private float levelWidth = 1f;
		public float spaceBetweenPoints = 5f;

		[Header("Gizmos")]
		[SerializeField] private bool showWayPointsOnDeselect = true;

		[SerializeField] private bool showAcurateSizeOfWayPoints = false;
		[SerializeField] private bool showWayPointZoneOnDeselect = true;

		private List<GameObject> wayPoints = new List<GameObject>();

		private void Awake()
		{
			if (wayPoints.Count == 0) { GenerateWayPoints(); }
		}

		#region Generate/Remove WayPoints

		private void GenerateWayPoints()
		{
			int columns = Mathf.RoundToInt(levelWidth / spaceBetweenPoints);
			int rows = Mathf.RoundToInt(levelHeight / spaceBetweenPoints);

			List<GameObject> tempRowParents = new List<GameObject>();

			//Debug.Log($"columns: {columns}, rows: {rows}, total: {columns * rows}");

			GameObject tempRowParent = Instantiate(new GameObject(), new Vector3(-levelWidth / 2f, levelHeight / 2, 0f), Quaternion.identity, transform);
			tempRowParents.Add(tempRowParent);

			for (int i = 0; i < columns; i++)
			{
				GameObject newWayPoint = Instantiate(wayPoint, new Vector3((-levelWidth / 2) + spaceBetweenPoints * i, levelHeight / 2, 0f), Quaternion.identity, tempRowParent.transform);
				wayPoints.Add(newWayPoint);
			}

			for (int i = 0; i < rows; i++)
			{
				GameObject newRowParent = Instantiate(tempRowParent, new Vector3(-levelWidth / 2, ((levelHeight / 2) - spaceBetweenPoints * i) - spaceBetweenPoints, 0f), Quaternion.identity, transform);

				if (transform.childCount % 2 == 0) { newRowParent.transform.position += new Vector3(spaceBetweenPoints / 2, 0f, 0f); }

				for (int j = 0; j < newRowParent.transform.childCount; j++)
				{
					wayPoints.Add(newRowParent.transform.GetChild(j).gameObject);
				}

				tempRowParents.Add(newRowParent);
			}

			foreach (var tempParent in tempRowParents)
			{
				tempParent.transform.DetachChildren();
				DestroyImmediate(tempParent);
			}
			foreach (var waypoint in wayPoints)
			{
				waypoint.transform.parent = transform;

				WayPoint controller = waypoint.GetComponent<WayPoint>();
				controller.hitboxRadius = spaceBetweenPoints / 2;
				controller.Active = true;
			}

			transform.rotation = Quaternion.identity;
		}

		//private void ClearWayPoints()
		//{
		//    foreach (var waypoint in wayPoints)
		//    {
		//        DestroyImmediate(waypoint);
		//    }
		//}

		#endregion Generate/Remove WayPoints

		#region public Get Functions

		public WayPoint GetClosestWayPoint(Vector3 position)
		{
			var closestWayPoint = wayPoints[0];

			foreach (var waypoint in wayPoints)
			{
				if (Vector3.Distance(position, waypoint.transform.position) < Vector3.Distance(position, closestWayPoint.transform.position))
				{
					closestWayPoint = waypoint;
				}
			}

			return closestWayPoint.GetComponent<WayPoint>();
		}

		public WayPoint[] GetNeighborWayPoints(WayPoint wayPoint)
		{
			WayPoint[] neighbors = new WayPoint[6];

			neighbors[0] = GetClosestWayPoint(wayPoint.position + new Vector3(-(spaceBetweenPoints / 2f), spaceBetweenPoints, 0f));
			neighbors[1] = GetClosestWayPoint(wayPoint.position + new Vector3((spaceBetweenPoints / 2f), spaceBetweenPoints, 0f));

			neighbors[2] = GetClosestWayPoint(wayPoint.position + new Vector3((spaceBetweenPoints), 0f, 0f));

			neighbors[3] = GetClosestWayPoint(wayPoint.position + new Vector3((spaceBetweenPoints / 2f), -spaceBetweenPoints, 0f));
			neighbors[4] = GetClosestWayPoint(wayPoint.position + new Vector3(-(spaceBetweenPoints / 2f), -spaceBetweenPoints, 0f));

			neighbors[5] = GetClosestWayPoint(wayPoint.position + new Vector3(-(spaceBetweenPoints), 0f, 0f));

			return neighbors;
		}

		#endregion public Get Functions

		#region Gizmos

		private void OnDrawGizmosSelected()
		{
			DrawWayPoints();
			DrawWayPointZone();
		}

		private void OnDrawGizmos()
		{
			if (showWayPointsOnDeselect) { DrawWayPoints(); }
			if (showWayPointZoneOnDeselect) { DrawWayPointZone(); }
		}

		private void DrawWayPoints()
		{
			foreach (var waypoint in wayPoints)
			{
				float circleRaduis = 1f;
				Color gizmoColor = Color.green;

				if (showAcurateSizeOfWayPoints) { circleRaduis = spaceBetweenPoints / 2; }

				if (waypoint.GetComponent<WayPoint>().Active == false) { gizmoColor = Color.red; }

				Gizmos.color = gizmoColor;
				Gizmos.DrawWireSphere(waypoint.transform.position, circleRaduis);
			}
		}

		private void DrawWayPointZone()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(transform.position, new Vector3(levelWidth, levelHeight, 1f));
		}

		#endregion Gizmos
	}
}