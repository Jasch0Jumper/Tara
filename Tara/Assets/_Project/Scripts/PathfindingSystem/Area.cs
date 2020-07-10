using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	[Serializable]
	public class Area
	{
		public Vector2 CornerA => new Vector2(Center.x - (Width / 2), Center.y - (Height / 2));
		public Vector2 CornerB => new Vector2(Center.x + (Width / 2), Center.y + (Height / 2));

		public float Width;
		public float Height;

		public Vector3 Offset;

		private Vector3 _center;
		public Vector3 Center { get => _center + Offset; }

		public void MoveCenter(Vector3 position) => _center = position;

		public List<Vector3> GetPointsInArea(float spaceBetweenPoints)
		{
			var points = new List<Vector3>();

			for (float x = CornerA.x; x < CornerB.x; x += spaceBetweenPoints)
			{
				for (float y = CornerA.y; y < CornerB.y; y += spaceBetweenPoints)
				{
					points.Add(new Vector2(x, y));
				}
			}

			return points;
		}
	}
}
