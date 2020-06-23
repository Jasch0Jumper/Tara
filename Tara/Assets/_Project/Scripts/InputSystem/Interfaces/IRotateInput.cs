using UnityEngine;

namespace Tara.InputSystem
{
	public interface IRotateInput
	{
		Vector2 GetTargetRotationPosition();

		bool LookAtMouse();
	}
}