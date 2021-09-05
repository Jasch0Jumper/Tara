namespace CITools
{
	public class Grid<T>
	{
		public T[,] Cells { get; set; }
		
		public T this[int x, int y]
		{
			get => Cells[x, y];
			set => Cells[x, y] = value;
		}

		private int _width;
		private int _height;

		public Grid(int width, int height)
		{
			_width = width;
			_height = height;

			Cells = new T[_width, _height];
		}
	}
}
