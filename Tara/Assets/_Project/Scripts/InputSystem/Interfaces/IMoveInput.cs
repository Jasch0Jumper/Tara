using UnityEngine;

namespace Tara.InputSystem
{
	public interface IMoveInput
	{
		Vector2 Input { get; }

		float SpeedMultiplier { get; }
	}
}