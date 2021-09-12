namespace CITools
{
	public struct GridPosition
	{
#pragma warning disable IDE1006 // Naming Styles
		public int x { get; set; }
		public int y { get; set; }
#pragma warning restore IDE1006 // Naming Styles

		public GridPosition(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return $"({x}, {y})";
		}
	}
}
