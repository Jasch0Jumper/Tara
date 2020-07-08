using Boo.Lang;
using System;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	[Serializable]
	public struct Area 
	{
		public Vector2 CornerA => new Vector2(Center.x - (Width / 2), Center.y - (Height / 2));
		public Vector2 CornerB => new Vector2(Center.x + (Width / 2), Center.y + (Height / 2));

		public float Width;
		public float Height;

		public Vector2 Center;
		public Vector3 Vector3Center => new Vector3(Center.x, Center.y, 0f);


		public Area(Vector2 center, Vector2 size)
		{
			Width = size.x;
			Height = size.y;

			Center = center;
		}

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
