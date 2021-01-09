using UnityEngine;

namespace Tara
{
	public interface IAIInput
	{
		Vector3 GetTargetPosition();

		bool UseFastSpeed();
		bool UseSlowSpeed();

		bool CanShoot();

		bool HasArrived();
	}
}