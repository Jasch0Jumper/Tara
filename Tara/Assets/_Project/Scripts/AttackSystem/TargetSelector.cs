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

		private List<Target> _targetsInStoppingRange = new List<Target>();
		private List<Target> _targetsInShootingRange = new List<Target>();
		private List<Target> _targetsInTargetingRange = new List<Target>();

		private TargetList _targetList;
		private RangeCollider[] _rangeColliders = new RangeCollider[3];

		private Target _currentTarget;

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
			_targetList = GetComponent<TargetList>();
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

				_rangeColliders[i] = ranges[i].AddComponent<RangeCollider>();
				_rangeColliders[i].RangeType = (RangeType)i;
			}
		}

		private void SubscribeToEvents()
		{
			foreach (var rangeCollider in _rangeColliders)
			{
				rangeCollider.OnRangeEnter += TargetEnteredRange;
				rangeCollider.OnRangeExit += TargetExitedRange;
			}

			_targetList.OnTargetRemovedFromList += RemoveTargetFromAllLists;
		}
		private void UnsubscribeFromEvents()
		{
			foreach (var rangeCollider in _rangeColliders)
			{
				rangeCollider.OnRangeEnter -= TargetEnteredRange;
				rangeCollider.OnRangeExit -= TargetExitedRange;
			}

			_targetList.OnTargetRemovedFromList -= RemoveTargetFromAllLists;
		}
		#endregion

		#region IAIInput implementation
		public Vector3 GetTargetPosition()
		{
			if (_currentTarget == null) { return transform.position; }
			return _currentTarget.transform.position;
		}

		public bool UseFastSpeed() => IsInTargetingRange(_currentTarget) && _targetList.IsHighPriority(_currentTarget);
		public bool UseSlowSpeed() => IsInTargetingRange(_currentTarget) == false;

		public bool CanShoot() => IsInShootingRange(_currentTarget);

		public bool HasArrived() => IsInStoppingRange(_currentTarget);
		#endregion

		#region Target Selection
		private void OnTargetsChanged() => SelectNewTarget();
		private void SelectNewTarget()
		{
			_currentTarget = GetClosestTarget();
		}
		#endregion

		#region Target List Managment
		private void TargetEnteredRange(RangeType rangeType, Target target)
		{
			switch (rangeType)
			{
				case RangeType.StoppingRange:
					_targetsInStoppingRange.Add(target); break;
				case RangeType.ShootingRange:
					_targetsInShootingRange.Add(target); break;
				case RangeType.TargetingRange:
					_targetsInTargetingRange.Add(target); break;
			}
			OnTargetsChanged();
		}
		private void TargetExitedRange(RangeType rangeType, Target target)
		{
			switch (rangeType)
			{
				case RangeType.StoppingRange:
					_targetsInStoppingRange.Remove(target); break;
				case RangeType.ShootingRange:
					_targetsInShootingRange.Remove(target); break;
				case RangeType.TargetingRange:
					_targetsInTargetingRange.Remove(target); break;
			}
			OnTargetsChanged();
		}

		private void RemoveTargetFromAllLists(Target target)
		{
			_targetsInStoppingRange.Remove(target);
			_targetsInShootingRange.Remove(target);
			_targetsInTargetingRange.Remove(target);

			OnTargetsChanged();
		}
		#endregion

		#region helper Methods
		private bool IsInStoppingRange(Target target) => _targetsInStoppingRange.Contains(target);
		private bool IsInShootingRange(Target target) => _targetsInShootingRange.Contains(target);
		private bool IsInTargetingRange(Target target) => _targetsInTargetingRange.Contains(target);
		
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
			List<Target> validTargets = _targetList.GetTargets(GetHighestPriorityLevel(_targetsInTargetingRange));

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
#if UNITY_EDITOR

		[Header("Gizmos")]
		[SerializeField] private bool showCurrentTargetOnDeselect = default;
		[SerializeField] private bool drawRanges = default;
		
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
			if (_currentTarget != null)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(transform.position, _currentTarget.transform.position);
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

#endif
		#endregion
	}
}
