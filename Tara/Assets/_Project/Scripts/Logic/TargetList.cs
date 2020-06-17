using System.Collections.Generic;
using UnityEngine;

namespace Tara
{
	public class TargetList : MonoBehaviour
	{
		[Header("Targets")]
		[Tooltip("Targets of this Type will automatically be added to the List of Targets.")]
		[SerializeField] private List<EntityType> targetTypes = new List<EntityType>();

		public List<Target> Targets = new List<Target>();

		[Header("Priority")]
		[Tooltip("At what priority does it cound as high priority?")]
		[SerializeField] private int priorityThreshold = 0;
		[Tooltip("The minimum priority Level a Target has to have to not be ignored")]
		[SerializeField] private int minPriorityLevel = 0;

		[Header("Gizmos")]
		[SerializeField] private bool showAllTargets = default;

		private TargetManager targetManager;

		public delegate void ListChanged(Target target);
		public event ListChanged OnTargetRemovedFromList;

		#region Setup and Callback

		private void Awake()
		{
			targetManager = FindObjectOfType<TargetManager>();
		}

		private void Start()
		{
			AddExistingTargets();
		}

		private void OnEnable()
		{
			TargetManager.OnTargetAdded += AddToTargets;
			TargetManager.OnTargetRemoved += RemoveFromTargets;
		}

		private void OnDisable()
		{
			TargetManager.OnTargetAdded -= AddToTargets;
			TargetManager.OnTargetRemoved -= RemoveFromTargets;
		}

		#endregion Setup and Callback

		#region List Managment

		private void AddExistingTargets()
		{
			foreach (var target in targetManager.Targets)
			{
				AddToTargets(target);
			}
		}

		private void AddToTargets(Target target)
		{
			if (targetTypes.Contains(target.GetComponent<Entity>().Type) && IsInPriorityRange(target))
			{ Targets.Add(target); }
		}

		private void RemoveFromTargets(Target target)
		{
			Targets.Remove(target);
			OnTargetRemovedFromList?.Invoke(target);
		}

		private bool IsInPriorityRange(Target target) => target.PriorityLevel >= minPriorityLevel;

		#endregion List Managment

		#region Filter Methods

		public List<Target> GetTargetsWithPriorityLevel(int priorityLevel)
		{
			List<Target> validTargets = new List<Target>(Targets);

			for (int i = 0; i < validTargets.Count; i++)
			{
				Target target = validTargets[i];
				if (target.PriorityLevel < priorityLevel) { validTargets.Remove(target); }
			}

			return validTargets;
		}

		public bool IsHighPriority(Target target) => target.PriorityLevel > priorityThreshold;

		public Target GetRandomTargetFromList(List<Target> potentialTargets)
		{
			Target randomTarget;

			if (potentialTargets.Count == 1) { randomTarget = potentialTargets[0]; }
			else { randomTarget = potentialTargets[Random.Range(0, potentialTargets.Count - 1)]; }

			return randomTarget;
		}

		#endregion

		#region Gizmos

		private void OnDrawGizmos()
		{
			if (showAllTargets) { DrawAllTargets(); }
		}

		private void DrawAllTargets()
		{
			if (Targets == null) { return; }

			foreach (var target in Targets)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireSphere(target.transform.position, 2.5f);
			}
		}

		#endregion
	}
}