using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tara.Pathfinding
{
	[Serializable]
	public class BlockedPoints
	{
		[SerializeField] private Vector3[] points;
		
		private Vector3 offset;

		public void Move(Vector3 position)
		{
			if (points.Length < 1) return;

			offset = position;
		}

		public List<Vector3> GetPointsInArea(float spaceBetweenPoints)
		{
			var allPoints = GetPointsWithOffset();
			
			if (points.Length < 1) return allPoints;

			allPoints.Add(GetPointWithOffset(0));

			for (int i = 0; i < allPoints.Count - 1; i++)
			{
				if (Vector3.Distance(allPoints[i], allPoints[i + 1]) > spaceBetweenPoints)
				{
					InsertPoint(allPoints, i, spaceBetweenPoints);
				}
			}

			return allPoints;
		}

		public List<Vector3> GetPointsWithOffset()
		{
			var pointsWithOffset = new List<Vector3>();

			foreach (var point in points)
			{
				pointsWithOffset.Add(point + offset);
			}

			return pointsWithOffset;
		}

		private Vector3 GetPointWithOffset(int index) => points[index] + offset;

		private void InsertPoint(List<Vector3> list, int index, float spacing)
		{
			Vector3 pointA = list[index];
			Vector3 pointB = list[index + 1];

			Vector3 newPoint = Vector3.MoveTowards(pointA, pointB, spacing);
			
			list.Insert(index + 1, newPoint);
		}
	}
}
