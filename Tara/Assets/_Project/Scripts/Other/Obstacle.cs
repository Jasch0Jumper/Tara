using System.Collections.Generic;
using UnityEngine;
using Tara.AttackSystem;
using Tara.PathfindingSystem;

namespace Tara
{
	[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
	public class Obstacle : MonoBehaviour, ICanCollideWithProjectiles
	{
		private List<WayPoint> _blockedWayPoints = new List<WayPoint>();

		private void OnTriggerStay2D(Collider2D collision)
		{
			WayPoint wayPoint = collision.GetComponent<WayPoint>();

			if (wayPoint != null)
			{
				wayPoint.Active = false;
				_blockedWayPoints.Add(wayPoint);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			WayPoint wayPoint = collision.GetComponent<WayPoint>();

			if (_blockedWayPoints.Contains(wayPoint))
			{
				wayPoint.Active = true;
				_blockedWayPoints.Remove(wayPoint);
			}
		}

		private void OnDisable()
		{
			foreach (var wayPoint in _blockedWayPoints)
			{
				wayPoint.Active = true;
			}
		}
	}
}