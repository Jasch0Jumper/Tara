using System.Collections.Generic;
using UnityEngine;

namespace Tara
{
	[RequireComponent(typeof(TargetFilter))]
	public class TargetSelector : MonoBehaviour, IAIInput
	{
		[Header("Gizmos")]
		[SerializeField] private bool showCurrentTargetOnDeselect = default;

		private TargetFilter targetFilter;
		private Target currentTarget;

		private void Awake()
		{
			targetFilter = GetComponent<TargetFilter>();
		}

		#region IAIInput implementation

		public Vector3 GetTargetPosition()
		{
			if (currentTarget == null) { return transform.position; }
			return currentTarget.transform.position;
		}

		public bool UseFastSpeed() => targetFilter.IsInTargetingRange(currentTarget) && targetFilter.IsHighPriority(currentTarget);
		public bool UseSlowSpeed() => targetFilter.IsInTargetingRange(currentTarget) == false;

		public bool CanShoot() => targetFilter.IsInShootingRange(currentTarget);

		public bool HasArrived() => targetFilter.IsInStoppingRange(currentTarget);

		#endregion 

		#region Target Selection

		public void SelectNewTarget()
		{
			currentTarget = targetFilter.GetClosestTarget();
		}

		#endregion 

		#region Gizmos

		private void OnDrawGizmosSelected()
		{
			DrawTargetLine();
		}
		private void OnDrawGizmos()
		{
			if (showCurrentTargetOnDeselect) { DrawTargetLine(); }	
		}

		private void DrawTargetLine()
		{
			if (currentTarget != null)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(transform.position, currentTarget.transform.position);
			}
		}
		
		#endregion
	}
}
