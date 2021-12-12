using UnityEngine;

namespace Tara
{
    public static class ExtensionMethods
    {
        public static float RotateWith(this float angel, Component component)
        {
            return angel + component.transform.rotation.eulerAngles.z;
        }
    }
}
