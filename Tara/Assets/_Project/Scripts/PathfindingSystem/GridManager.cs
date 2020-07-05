using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tara.PathfindingSystem
{
	public class GridManager : MonoBehaviour
	{
		[SerializeField] private int width = default;
		[SerializeField] private int height = default;
		[SerializeField] private float cellSize = default;

		private Grid _grid;

		private void Start()
		{
			_grid = new Grid(width, height, cellSize);
		}
	}
}


