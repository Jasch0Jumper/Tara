using UnityEngine;

namespace Tara
{
    public static class ExtensionMethods
    {
		public static Vector3 AsVector3(this Vector2 vector2) => new Vector3(vector2.x, vector2.y, 0f);
	}
}