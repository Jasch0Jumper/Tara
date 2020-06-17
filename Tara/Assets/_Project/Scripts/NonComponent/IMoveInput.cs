using UnityEngine;

namespace Tara
{
	public interface IMoveInput
	{
		Vector2 GetInput();

		float GetSpeedMultiplier();
	}
}