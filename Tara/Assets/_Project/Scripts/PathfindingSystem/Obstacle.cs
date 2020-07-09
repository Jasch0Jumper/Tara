using UnityEngine;
using System.Collections.Generic;

namespace Tara.PathfindingSystem 
{
	public class Obstacle : MonoBehaviour
	{
		[SerializeField] private List<Area> blockedAreas = new List<Area>();
		[Header("Gizmos")]
		[SerializeField] private bool showBlockedArea = default;
		[SerializeField] private bool showblockedPoints = default;
		[SerializeField] private bool showCornersOfAreas = default;

		private GridManager _gridManager;

		private void Awake()
		{
			_gridManager = FindObjectOfType<GridManager>();
		}
		private void Start()
		{
			BlockPath();
		}

		[ContextMenu("BlockPath")]
		private void BlockPath()
		{
			_gridManager.BlockGridArea(blockedAreas);
		}

		#region Gizmos
		private void OnDrawGizmos()
		{
			if (showBlockedArea) { DrawBlockedArea(); }
			if (showblockedPoints) { DrawBlockedPoints(); }
			if (showCornersOfAreas) { DrawCornersOfBlockedAreas(); }
		}
		private void OnDrawGizmosSelected()
		{
			DrawBlockedArea();
			DrawBlockedPoints();
			DrawCornersOfBlockedAreas();
		}

		private void DrawBlockedArea()
		{
			if (blockedAreas.Count <= 0) return;

			Gizmos.color = Color.red;
			
			foreach (var area in blockedAreas)
			{
				Vector3 xOne = new Vector3(1f, 0f, 0f);
				Vector3 yOne = new Vector3(0f, 1f, 0f);
				Gizmos.DrawLine(area.Vector3Center - xOne, area.Vector3Center + xOne);
				Gizmos.DrawLine(area.Vector3Center - yOne, area.Vector3Center + yOne);

				Gizmos.DrawWireCube(area.Center, new Vector3(area.Width, area.Height, 1f));
			}
		}
		private void DrawBlockedPoints()
		{
			if (blockedAreas.Count <= 0) return;
			
			Gizmos.color = Color.yellow;

			foreach (var area in blockedAreas)
			{
				foreach (var blockedPoint in area.GetPointsInArea(GridManager.CELLSIZE))
				{
					Gizmos.DrawWireSphere(blockedPoint, 1f);
				}
			}
		}
		private void DrawCornersOfBlockedAreas()
		{
			Gizmos.color = Color.cyan;
			
			foreach (var area in blockedAreas)
			{
				Gizmos.DrawWireSphere(area.CornerA, 0.5f);
				Gizmos.DrawWireSphere(area.CornerB, 0.5f);
			}
		}
		#endregion
	}
}
