using UnityEngine;

namespace Tara
{
    public static class ExtensionMethods
    {
		public static Vector3 AsVector3(this Vector2 vector) => new Vector3(vector.x, vector.y, 0f);
		public static Vector2 AsVector2(this Vector3 vector) => new Vector2(vector.x, vector.y);
	}
}