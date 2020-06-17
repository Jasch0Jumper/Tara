using System.Collections.Generic;
using UnityEngine;

namespace Tara
{
	[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
	public class Obstacle : MonoBehaviour, ICanCollideWithProjectiles
	{
		private List<WayPoint> blockedWayPoints = new List<WayPoint>();

		private void OnTriggerStay2D(Collider2D collision)
		{
			WayPoint wayPoint = collision.GetComponent<WayPoint>();

			if (wayPoint != null)
			{
				wayPoint.Active = false;
				blockedWayPoints.Add(wayPoint);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			WayPoint wayPoint = collision.GetComponent<WayPoint>();

			if (blockedWayPoints.Contains(wayPoint))
			{
				wayPoint.Active = true;
				blockedWayPoints.Remove(wayPoint);
			}
		}

		private void OnDisable()
		{
			foreach (var wayPoint in blockedWayPoints)
			{
				wayPoint.Active = true;
			}
		}
	}
}