using UnityEngine;

namespace Tara
{
	public interface IMoveInput
	{
		Vector2 Input { get; }

		float SpeedMultiplier { get; }
	}
}