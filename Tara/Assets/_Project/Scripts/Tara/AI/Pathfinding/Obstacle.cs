using UnityEngine;
using CI.Pathfinding;

namespace Tara.Pathfinding
{
	public class Obstacle : MonoBehaviour
	{
		[SerializeField] private BlockedPoints blockedArea;

		private Vector3 _previousPosition;

		private GridBehaviour _grid;

		private void Awake()
		{
			_grid = FindObjectOfType<GridBehaviour>();
		}
		private void OnEnable()
		{
			Move();
			_grid.Block(blockedArea);
		}
		private void OnDisable()
		{
			_grid.UnBlock(blockedArea);
		}
		private void Update()
		{
			if (_previousPosition == transform.position) return;

			_grid.UnBlock(blockedArea);

			Move();

			_previousPosition = transform.position;

			_grid.Block(blockedArea);
		}

		private void Move() => blockedArea.Move(transform.position);

		#region Gizmos
#if UNITY_EDITOR

		[Header("Gizmos")]
		[SerializeField] private bool showPoints = default;
		
		private void OnDrawGizmos()
		{
			if (showPoints) { DrawPoints(); }
		}
		private void OnDrawGizmosSelected()
		{
			DrawPoints();
		}

		private void DrawPoints()
		{
			Gizmos.color = Color.cyan;

			foreach (var point in blockedArea.GetPointsWithOffset())
			{
				Gizmos.DrawWireSphere(point, 1f);
			}
		}
#endif
		#endregion
	}
}
