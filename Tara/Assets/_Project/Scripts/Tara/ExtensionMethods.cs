using UnityEngine;

namespace Tara
{
    public static class ExtensionMethods
    {
		public static Vector2 ToVector2(this Vector3 vector) => new Vector2(vector.x, vector.y);
		public static Vector3 ToVector3(this Vector2 vector) => new Vector3(vector.x, vector.y, 0f);
		public static Vector3 ToVector3(this Vector2 vector, float z) => new Vector3(vector.x, vector.y, z);

		public static Vector3 RelativeTo(this Vector3 vector, MonoBehaviour monoBehaviour)
		{
			return monoBehaviour.transform.position + vector;
		}
		public static Vector2 RelativeTo(this Vector2 vector, Component component)
		{
			return component.transform.position + vector.ToVector3();
		}

		public static float RotateWith(this float angel, Component component)
		{
			return angel + component.transform.rotation.eulerAngles.z;
		}
	}
}