using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	[Serializable]
	public class BlockPointChain
	{
		[SerializeField] private List<Coordinate> points = new List<Coordinate>();
		public List<Coordinate> Points => points;

		public void Move(Coordinate position)
		{
			foreach (var point in points)
			{
				point.SetTo(position);
			}
		}

		public List<Coordinate> GetPointsInArea(float spaceBetweenPoints)
		{
			var allPoints = new List<Coordinate>(points)
			{
				Coordinate.Zero
			};

			if (spaceBetweenPoints < 1f) { return allPoints; }

			for (int i = 0; i < allPoints.Count - 1; i++)
			{
				if (Coordinate.Distance(allPoints[i], allPoints[i + 1]) > spaceBetweenPoints)
				{
					InsertPoint(allPoints, i, spaceBetweenPoints);
				}
			}

			return allPoints;
		}

		private void InsertPoint(List<Coordinate> list, int index, float spacing)
		{
			Coordinate pointA = list[index];
			Coordinate pointB = list[index + 1];

			Coordinate newPoint = Coordinate.MoveTowards(pointA, pointB, spacing);
			
			list.Insert(index + 1, newPoint);
		}
	}
}
