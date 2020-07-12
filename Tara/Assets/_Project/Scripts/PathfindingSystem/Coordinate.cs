using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	[Serializable]
	public class Coordinate
	{
		public float x;
		public float y;
		public float z;

		public Coordinate(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static Coordinate Zero => new Coordinate(0f, 0f, 0f);

		#region Conversion
		public Vector3 Vector3 => ToVector3(this);
		public Vector2 Vector2 => ToVector2(this);

		public static Coordinate Convert(Vector3 vector3) => new Coordinate(vector3.x, vector3.y, vector3.z);
		public static Coordinate Convert(Vector2 vector2) => new Coordinate(vector2.x, vector2.y, 0f);

		public static Vector3 ToVector3(Coordinate coordinate) => new Vector3(coordinate.x, coordinate.y, coordinate.z);
		public static Vector2 ToVector2(Coordinate coordinate) => new Vector2(coordinate.x, coordinate.y);
		#endregion

		#region Methods
		public Coordinate SetTo(Coordinate coordinate)
		{
			x = coordinate.x;
			y = coordinate.y;
			z = coordinate.z;
			return this;
		}

		public static float Distance(Coordinate a, Coordinate b)
		{
			float num = a.x - b.x;
			float num2 = a.y - b.y;
			float num3 = a.z - b.z;
			return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
		}
		public static Coordinate MoveTowards(Coordinate current, Coordinate target, float maxDistanceDelta)
		{
			float num = target.x - current.x;
			float num2 = target.y - current.y;
			float num3 = target.z - current.z;
			float num4 = num * num + num2 * num2 + num3 * num3;
			if (num4 == 0f || (maxDistanceDelta >= 0f && num4 <= maxDistanceDelta * maxDistanceDelta))
			{
				return target;
			}

			float num5 = (float)Math.Sqrt(num4);
			return new Coordinate(current.x + num / num5 * maxDistanceDelta, current.y + num2 / num5 * maxDistanceDelta, current.z + num3 / num5 * maxDistanceDelta);
		}
		#endregion

		#region Operators
		public static Coordinate operator +(Coordinate a, Coordinate b) => new Coordinate(a.x + b.x, a.y + b.y, a.z + b.z);
		public static Coordinate operator -(Coordinate a, Coordinate b) => new Coordinate(a.x - b.x, a.y - b.y, a.z - b.z);
		public static Coordinate operator -(Coordinate a) => new Coordinate(0f - a.x, 0f - a.y, 0f - a.z);
		public static Coordinate operator *(Coordinate a, float d) => new Coordinate(a.x * d, a.y * d, a.z * d);
		public static Coordinate operator *(float d, Coordinate a) => new Coordinate(a.x * d, a.y * d, a.z * d);
		public static Coordinate operator /(Coordinate a, float d) => new Coordinate(a.x / d, a.y / d, a.z / d);
		public static bool operator ==(Coordinate lhs, Coordinate rhs)
		{
			float num = lhs.x - rhs.x;
			float num2 = lhs.y - rhs.y;
			float num3 = lhs.z - rhs.z;
			float num4 = num * num + num2 * num2 + num3 * num3;
			return num4 < 9.99999944E-11f;
		}
		public static bool operator !=(Coordinate lhs, Coordinate rhs) => !(lhs == rhs);
		#endregion
		#region Equals() and GetHashCode()
		public override bool Equals(object obj)
		{
			return obj is Coordinate coordinate &&
				   x == coordinate.x &&
				   y == coordinate.y &&
				   z == coordinate.z;
		}
		public override int GetHashCode()
		{
			int hashCode = -307843816;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			hashCode = hashCode * -1521134295 + z.GetHashCode();
			return hashCode;
		}
		#endregion
	}
}
