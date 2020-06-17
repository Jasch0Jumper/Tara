using System.Collections.Generic;
using UnityEngine;

namespace Tara
{
	public class TargetManager : MonoBehaviour
	{
		[Header("Gizmos")]
		[SerializeField] private bool showAllTargetsOnDeselect = default;

		private bool initialized;

		public List<Target> Targets { get; } = new List<Target>();

		public delegate void TargetListUpdate(Target target);

		public static event TargetListUpdate OnTargetAdded;

		public static event TargetListUpdate OnTargetRemoved;

		private void Awake()
		{
			initialized = true;
		}

		// Add OnEnable & OnDisable
		// send Event to register & unregister all targets
		// used when this object is disabled to "pause" target managment

		public void RegisterAsTargetAble(Target target)
		{
			if (Targets.Contains(target) == false)
			{
				Targets.Add(target);
				OnTargetAdded?.Invoke(target);
			}
		}

		public void UnregisterAsTargetAble(Target target)
		{
			Targets.Remove(target);
			OnTargetRemoved?.Invoke(target);
		}

		#region Gizmos

		private void OnDrawGizmos()
		{
			if (showAllTargetsOnDeselect) { DrawAllTargets(); }
		}

		private void OnDrawGizmosSelected()
		{
			DrawAllTargets();
		}

		private void DrawAllTargets()
		{
			if (initialized == false) { return; }

			Gizmos.color = Color.red;

			for (int i = 0; i < Targets.Count; i++)
			{
				Target target = Targets[i];
				Gizmos.DrawWireSphere(target.transform.position, 1f);
			}
		}

		#endregion Gizmos
	}
}