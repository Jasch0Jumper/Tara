using UnityEngine;

namespace Tara
{
	public interface IRotateInput
	{
		Vector2 GetTargetRotationPosition();

		bool LookAtMouse();
	}
}