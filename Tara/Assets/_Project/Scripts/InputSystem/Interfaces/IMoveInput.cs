using UnityEngine;

namespace Tara.InputSystem
{
	public interface IMoveInput
	{
		Vector2 GetInput();
		
		float GetSpeedMultiplier();
	}
}