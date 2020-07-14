using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	[Serializable]
	public class BlockPointChain
	{
		[SerializeField] private List<Vector3> points = new List<Vector3>();
		private Vector3 center = default;

		public void MovePoints(Vector3 position)
		{
			if (points.Count < 1) { return; }

			center = position;
		}

		public List<Vector3> GetPointsInArea(float spaceBetweenPoints)
		{
			var allPoints = GetPoints();
			
			if (points.Count < 1) { return allPoints; }

			allPoints.Add(GetPointAt(0));

			if (spaceBetweenPoints < 1f) { return allPoints; }

			for (int i = 0; i < allPoints.Count - 1; i++)
			{
				if (Vector3.Distance(allPoints[i], allPoints[i + 1]) > spaceBetweenPoints)
				{
					InsertPoint(allPoints, i, spaceBetweenPoints);
				}
			}

			return allPoints;
		}

		public Vector3 GetPointAt(int index) => points[index] + center;
		public List<Vector3> GetPoints()
		{
			List<Vector3> allPoints = new List<Vector3>();

			for (int i = 0; i < points.Count; i++)
			{
				allPoints.Add(GetPointAt(i));
			}

			return allPoints;
		}

		private void InsertPoint(List<Vector3> list, int index, float spacing)
		{
			Vector3 pointA = list[index];
			Vector3 pointB = list[index + 1];

			Vector3 newPoint = Vector3.MoveTowards(pointA, pointB, spacing);
			
			list.Insert(index + 1, newPoint);
		}
	}
}
