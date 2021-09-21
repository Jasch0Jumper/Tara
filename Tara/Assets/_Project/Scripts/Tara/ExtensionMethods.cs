using UnityEngine;

namespace Tara
{
    public static class ExtensionMethods
    {
		public static Vector3 AsVector3(this Vector2 vector) => new Vector3(vector.x, vector.y, 0f);
		public static Vector2 AsVector2(this Vector3 vector) => new Vector2(vector.x, vector.y);

		public static Vector3 RelativeTo(this Vector3 vector, MonoBehaviour monoBehaviour)
		{
			return monoBehaviour.transform.position + vector;
		}
		public static Vector2 RelativeTo(this Vector2 vector, Component component)
		{
			return component.transform.position + vector.AsVector3();
		}

		public static float RotateWith(this float angel, Component component)
		{
			return angel + component.transform.rotation.eulerAngles.z;
		}
	}
}