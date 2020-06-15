using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TargetList))]
public class TargetSelector : MonoBehaviour, IAIInput
{
	[Header("Ranges: 0 = Stopping, 1 = Shooting, 2 = Targeting")]
	[SerializeField] [Range(5f, 100f)] private float[] rangeRadius = new float[3];

	[Header("Gizmos")]
	[SerializeField] private bool enableGizmos = default;
	[SerializeField] private bool showRangeOnDeselect = default;
	[SerializeField] private bool showCurrentTargetOnDeselect = default;
	[SerializeField] private bool showAllTargets = default;

	private TargetList targetList;
	private Target currentTarget;

	private List<Target> targetsInStoppingRange = new List<Target>();
	private List<Target> targetsInShootingRange = new List<Target>();
	private List<Target> targetsInTargetingRange = new List<Target>();

	#region Setup and Callback

	private void Awake()
	{
		targetList = GetComponent<TargetList>();
		GenerateCollidersForRanges();
	}

	private void OnEnable()
	{
		RangeCollider.OnRangeEnter += TargetEnteredRange;
		RangeCollider.OnRangeExit += TargetExitedRange;

		targetList.OnTargetRemovedFromList += RemoveTargetFromAllLists;
	}
	private void OnDisable()
	{
		RangeCollider.OnRangeEnter -= TargetEnteredRange;
		RangeCollider.OnRangeExit -= TargetExitedRange;

		targetList.OnTargetRemovedFromList -= RemoveTargetFromAllLists;
	}

	private void GenerateCollidersForRanges()
	{
		GameObject[] ranges = new GameObject[3];

		for (int i = 0; i < 3; i++)
		{
			ranges[i] = Instantiate(new GameObject(), transform);

			CircleCollider2D collider = ranges[i].AddComponent<CircleCollider2D>();
			collider.radius = rangeRadius[i];
			collider.isTrigger = true;

			RangeCollider rangeCollider = ranges[i].AddComponent<RangeCollider>();
			rangeCollider.RangeType = (RangeType)i;
			rangeCollider.TargetSelector = this;
		}
	}

	#endregion

	#region IAIInput implementation

	public Vector3 GetTargetPosition() 
	{ 
		if (currentTarget == null) { return transform.position; }
		return currentTarget.transform.position; 
	}

	public bool UseFastSpeed() => IsInTargetingRange(currentTarget) && targetList.IsHighPriority(currentTarget);
	public bool UseSlowSpeed() => IsInTargetingRange(currentTarget) == false;

	public bool CanShoot() => IsInShootingRange(currentTarget);

	public bool HasArrived() => IsInStoppingRange(currentTarget);

	#endregion

	#region Target Selection

	private void RefreshCurrentTarget()
	{
		if (IsNewTargetNeeded()) { SelectNewTarget(); }
	}
	private bool IsNewTargetNeeded()
	{
		if (currentTarget == null) { return true; }
		if (IsInTargetingRange(currentTarget) == false) { return true; }
		if (currentTarget.PriorityLevel < targetList.GetHighestPriorityLevelInList(targetsInTargetingRange)) { return true; }

		return false;
	}
	private void SelectNewTarget()
	{
		currentTarget = targetList.GetClosestTarget(targetList.GetHighestPriorityLevelInList(targetsInTargetingRange));
	}

	#endregion

	#region Target List Managment
	
	private void TargetEnteredRange(RangeType rangeType, Target target)
	{
		switch (rangeType)
		{
			case RangeType.StoppingRange:
				targetsInStoppingRange.Add(target); break;
			case RangeType.ShootingRange:
				targetsInShootingRange.Add(target); break;
			case RangeType.TargetingRange:
				targetsInTargetingRange.Add(target);
				RefreshCurrentTarget();
				break;
		}
	}
	private void TargetExitedRange(RangeType rangeType, Target target)
	{
		switch (rangeType)
		{
			case RangeType.StoppingRange:
				targetsInStoppingRange.Remove(target); 
				break;
			case RangeType.ShootingRange:
				targetsInShootingRange.Remove(target); 
				break;
			case RangeType.TargetingRange:
				targetsInTargetingRange.Remove(target);
				RefreshCurrentTarget();
				break;
		}
	}

	private void RemoveTargetFromAllLists(Target target)
	{
		targetsInStoppingRange.Remove(target);
		targetsInShootingRange.Remove(target);
		targetsInTargetingRange.Remove(target);

		RefreshCurrentTarget();
	}

	private bool IsInStoppingRange(Target target) => targetsInStoppingRange.Contains(target);
	private bool IsInShootingRange(Target target) => targetsInShootingRange.Contains(target);
	private bool IsInTargetingRange(Target target) => targetsInTargetingRange.Contains(target);

	#endregion

	#region Gizmos

	private void OnDrawGizmosSelected()
	{
		if (enableGizmos)
		{
			DrawRanges();
			DrawTargetLine();
			DrawAllTargets();
		}
	}
	private void OnDrawGizmos()
	{
		if (enableGizmos)
		{
			if (showRangeOnDeselect) { DrawRanges(); }
			if (showCurrentTargetOnDeselect) { DrawTargetLine(); }
			if (showAllTargets) { DrawAllTargets(); }
		}
	}

	private void DrawRanges()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, rangeRadius[0]);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, rangeRadius[1]);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, rangeRadius[2]);
	}
	private void DrawTargetLine()
	{
		if (currentTarget != null)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(transform.position, currentTarget.transform.position);
		}
	}
	private void DrawAllTargets()
	{
		if (targetList == null) { return; }

		foreach (var target in targetList.Targets)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(target.transform.position, 2.5f);
		}
	}

	#endregion
}
