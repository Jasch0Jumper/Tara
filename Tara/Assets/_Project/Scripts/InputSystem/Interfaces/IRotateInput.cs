using UnityEngine;

namespace Tara.InputSystem
{
	public interface IRotateInput
	{
		Vector2 TargetRotationPosition { get; }

		bool LookAtMouse { get; }
	}
}