using System.Collections.Generic;
using UnityEngine;

namespace Tara.Combat
{
	public class TargetList : MonoBehaviour
	{
		[Header("Targets")]
		[Tooltip("Targets of this Type will automatically be added to the List of Targets.")]
		[SerializeField] private List<EntityType> targetTypes = new List<EntityType>();

		public List<Target> Targets = new List<Target>();

		[Header("Priority")]
		[Tooltip("When does it count as high priority?")]
		[SerializeField] private int priorityThreshold = 0;
		[Tooltip("The minimum priority Level a Target has to have to not be ignored")]
		[SerializeField] private int minPriorityLevel = 0;


		private TargetManager _targetManager;

		public delegate void ListChanged(Target target);
		public event ListChanged OnTargetRemovedFromList;

		#region Setup and Callback

		private void Awake()
		{
			_targetManager = FindObjectOfType<TargetManager>();
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
			foreach (var target in _targetManager.Targets)
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

		#region public helper Methods

		public List<Target> GetTargets(int priorityLevel)
		{
			List<Target> potentialTargets = new List<Target>(Targets);
			List<Target> validTargets = new List<Target>();

			for (int i = 0; i < potentialTargets.Count; i++)
			{
				if (potentialTargets[i].PriorityLevel < priorityLevel) 
				{ 
					potentialTargets[i] = null;
				}
			}
			for (int i = potentialTargets.Count - 1; i > -1; i--)
			{
				if (potentialTargets[i] != null)
				{
					validTargets.Add(potentialTargets[i]);
				}
			}

			return validTargets;
		}
		
		public bool IsHighPriority(Target target) => target.PriorityLevel >= priorityThreshold;

		public Target GetRandomTargetFrom(List<Target> targetList)
		{
			Target randomTarget;
			randomTarget = targetList[Random.Range(0, targetList.Count - 1)];
			return randomTarget;
		}

		#endregion

		#region Gizmos
#if UNITY_EDITOR

		[Header("Gizmos")]
		[SerializeField] private bool showAllTargets = default;

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
#endif
		#endregion
	}
}
