using UnityEngine;

namespace Tara
{
	public interface IRotateInput
	{
		Vector2 TargetRotationPosition { get; }

		bool LookAtMouse { get; }
	}
}