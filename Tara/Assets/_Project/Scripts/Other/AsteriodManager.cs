using System.Collections.Generic;
using UnityEngine;

namespace Tara
{
	public class AsteriodManager : MonoBehaviour
	{
		[Range(1f, 300f)] public float MaxSpawnRange = 1f;
		[Range(1f, 300f)] public float MinSpawnRange = 1f;
		[Space]
		[SerializeField] [Range(1, 100)] private int maxAsteriodCount = default;
		[SerializeField] [Range(1f, 100f)] private float minDistance = 1f;
		[Space]
		[SerializeField] private GameObject asteriodPrefab = default;
		[Header("Gizmos")]
		[SerializeField] private bool drawOnDeselect = default;

		private List<Transform> _asteriodsTransforms = new List<Transform>();

		private void Start()
		{
			SummonAsteriods();
		}

		private void Update()
		{
			CleaList();
		}

		public void SummonAsteriods()
		{
			if (CausesInfinteLoop(MinSpawnRange, MaxSpawnRange, maxAsteriodCount, minDistance) != false)
			{ 
				Debug.LogError("Not enaugh Space for alle Asteroids! Change maxCount or minDistance!", this);
				return;
			}

			if (_asteriodsTransforms.Count < maxAsteriodCount)
			{
				int maxSpawnCount = maxAsteriodCount - _asteriodsTransforms.Count;
				for (int i = 0; i < maxSpawnCount; i++)
				{
					var newAsteriod = Instantiate(asteriodPrefab, GetCoordinate(MinSpawnRange, MaxSpawnRange), RandomRotation());

					_asteriodsTransforms.Add(newAsteriod.GetComponent<Transform>());
				}
			}
		}

		public void DestroyAsteriods()
		{
			foreach (var asteriod in _asteriodsTransforms)
			{
				DestroyImmediate(asteriod.gameObject);
			}
			CleaList();
		}

		public void ResetAsteriods()
		{
			DestroyAsteriods();
			SummonAsteriods();
		}

		#region private functions
		private Vector2 GetCoordinate(float minRange, float maxRange)
		{
			Vector2 coordinate;

			do
			{ coordinate = RandomCoordinate(minRange, maxRange); }
			while (IsCoordinateValid(coordinate) == false);

			return coordinate;
		}

		#region Validation
		private bool IsCoordinateValid(Vector2 coordinate)
		{
			foreach (var asteriod in _asteriodsTransforms)
			{
				if (Vector2.Distance(asteriod.position, coordinate) < minDistance)
				{ return false; }
			}
			return true;
		}

		private bool CausesInfinteLoop(float minRadius, float maxRadius, int maxCount, float minDistance)
		{
			float maxCircleArea = Mathf.PI * maxRadius * maxRadius;
			float minCircleArea = Mathf.PI * minRadius * minRadius;
			float spawnCircleArea = maxCircleArea - minCircleArea;
			float designatetArea = Mathf.PI * minDistance * minDistance * maxCount;

			if (spawnCircleArea < designatetArea) { return true; }

			return false;
		}
		#endregion 

		#region Random Rotation / Coordinates
		private Vector2 RandomCoordinate(float minRange, float maxRange)
		{
			transform.rotation = RandomRotation();
			transform.position = transform.up * Random.Range(minRange, maxRange);

			Vector2 coordinate = transform.position;

			transform.position = Vector3.zero;

			return coordinate;
		}
		private Quaternion RandomRotation()
		{
			return Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0f, 359f)));
		}
		#endregion 

		private void CleaList()
		{
			for (int i = _asteriodsTransforms.Count - 1; i > -1; i--)
			{
				if (_asteriodsTransforms[i] == null)
				{ _asteriodsTransforms.RemoveAt(i); }
			}
		}

		#endregion

		#region Gizmos

		private void OnDrawGizmos()
		{
			if (drawOnDeselect) { DrawRanges(); }
		}

		private void OnDrawGizmosSelected()
		{
			DrawRanges();
		}

		private void DrawRanges()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, MaxSpawnRange);
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(transform.position, MinSpawnRange);
		}

		#endregion Gizmos
	}
}