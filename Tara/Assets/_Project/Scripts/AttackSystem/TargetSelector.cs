using System;
using System.Collections.Generic;
using UnityEngine;
using Tara.InputSystem;

namespace Tara.AttackSystem
{
	[RequireComponent(typeof(TargetList))]
	public class TargetSelector : MonoBehaviour, IAIInput
	{
		[Header("Ranges")] [Tooltip("0 = Stopping, 1 = Shooting, 2 = Targeting")]
		[SerializeField] [Range(1f, 100f)] private float[] rangeRadius = new float[3];
		[Header("Gizmos")]
		[SerializeField] private bool showCurrentTargetOnDeselect = default;
		[SerializeField] private bool drawRanges = default;

		private List<Target> targetsInStoppingRange = new List<Target>();
		private List<Target> targetsInShootingRange = new List<Target>();
		private List<Target> targetsInTargetingRange = new List<Target>();

		private TargetList targetList;
		private RangeCollider[] rangeColliders = new RangeCollider[3];

		private Target currentTarget;

		#region Unity Event Methods
		private void Awake()
		{
			CacheComponents();
			GenerateCollidersForRanges();
		}

		private void OnEnable()
		{
			SubscribeToEvents();
		}

		private void OnDisable()
		{
			UnsubscribeFromEvents();
		}
		#endregion

		#region Methods called by Unity Event Methods
		private void CacheComponents()
		{
			targetList = GetComponent<TargetList>();
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

				rangeColliders[i] = ranges[i].AddComponent<RangeCollider>();
				rangeColliders[i].RangeType = (RangeType)i;
			}
		}

		private void SubscribeToEvents()
		{
			foreach (var rangeCollider in rangeColliders)
			{
				rangeCollider.OnRangeEnter += TargetEnteredRange;
				rangeCollider.OnRangeExit += TargetExitedRange;
			}

			targetList.OnTargetRemovedFromList += RemoveTargetFromAllLists;
		}
		private void UnsubscribeFromEvents()
		{
			foreach (var rangeCollider in rangeColliders)
			{
				rangeCollider.OnRangeEnter -= TargetEnteredRange;
				rangeCollider.OnRangeExit -= TargetExitedRange;
			}

			targetList.OnTargetRemovedFromList -= RemoveTargetFromAllLists;
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
		private void OnTargetsChanged() => SelectNewTarget();
		private void SelectNewTarget()
		{
			currentTarget = GetClosestTarget();
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
					targetsInTargetingRange.Add(target); break;
			}
			OnTargetsChanged();
		}
		private void TargetExitedRange(RangeType rangeType, Target target)
		{
			switch (rangeType)
			{
				case RangeType.StoppingRange:
					targetsInStoppingRange.Remove(target); break;
				case RangeType.ShootingRange:
					targetsInShootingRange.Remove(target); break;
				case RangeType.TargetingRange:
					targetsInTargetingRange.Remove(target); break;
			}
			OnTargetsChanged();
		}

		private void RemoveTargetFromAllLists(Target target)
		{
			targetsInStoppingRange.Remove(target);
			targetsInShootingRange.Remove(target);
			targetsInTargetingRange.Remove(target);

			OnTargetsChanged();
		}
		#endregion

		#region helper Methods
		private bool IsInStoppingRange(Target target) => targetsInStoppingRange.Contains(target);
		private bool IsInShootingRange(Target target) => targetsInShootingRange.Contains(target);
		private bool IsInTargetingRange(Target target) => targetsInTargetingRange.Contains(target);
		
		private int GetHighestPriorityLevel(List<Target> targets)
		{
			int highestPriorityLevel = 0;

			for (int i = 0; i < targets.Count; i++)
			{
				Target target = targets[i];
				if (target.PriorityLevel > highestPriorityLevel)
				{
					highestPriorityLevel = target.PriorityLevel;
				}
			}

			return highestPriorityLevel;
		}

		private Target GetClosestTarget()
		{
			Target target = null;
			List<Target> validTargets = targetList.GetTargets(GetHighestPriorityLevel(targetsInTargetingRange));

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
		#endregion

		#region Gizmos
		private void OnDrawGizmosSelected()
		{
			DrawTargetLine();
			DrawRanges();
		}
		private void OnDrawGizmos()
		{
			if (showCurrentTargetOnDeselect) { DrawTargetLine(); }	
			if (drawRanges) { DrawRanges(); }
		}

		private void DrawTargetLine()
		{
			if (currentTarget != null)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(transform.position, currentTarget.transform.position);
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
		#endregion
	}
}
