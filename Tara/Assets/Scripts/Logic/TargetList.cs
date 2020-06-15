using UnityEngine;
using System.Collections.Generic;

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

	#endregion

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

	#endregion

	#region public Methods

	public int GetHighestPriorityLevelInList(List<Target> targets)
	{
		int currentHighesPriorityLevel = 0;

		for (int i = 0; i < targets.Count; i++)
		{
			Target target = targets[i];
			if (target.PriorityLevel > currentHighesPriorityLevel)
			{
				currentHighesPriorityLevel = target.PriorityLevel;
			}
		}

		return currentHighesPriorityLevel;
		
	}
	public Target GetClosestTarget(int priorityLevel)
	{
		Target target = null;
		List<Target> validTargets = GetTargetsWithPriorityLevel(priorityLevel);

		if (validTargets.Count >= 1)
		{
			target = validTargets[0];

			for (int i = 0; i < validTargets.Count; i++)
			{
				Target newTarget = validTargets[i];

				float distanceToNewTarget = Vector2.Distance(transform.position, newTarget.transform.position);
				float distanceToCurrentTarget = Vector2.Distance(transform.position, target.transform.position);

				if (distanceToNewTarget < distanceToCurrentTarget)
				{ target = newTarget; }
			}
		}

		return target;
	}
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
	public Target GetRandomTargetFromList(List<Target> potentialTargets)
	{
		Target randomTarget;

		if (potentialTargets.Count == 1) { randomTarget = potentialTargets[0]; }
		else { randomTarget = potentialTargets[Random.Range(0, potentialTargets.Count - 1)]; }

		return randomTarget;
	}

	public bool IsHighPriority(Target target) => target.PriorityLevel > priorityThreshold;

	#endregion
}
