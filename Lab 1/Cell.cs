using System.Collections.Generic;

namespace PortableConsole
{
	public class Cell
	{
		public Vector2 Position;
		public bool IsAlive;
		public bool IsRemoveCandidate = true;
		public bool IsBornCandidate;
		public bool NotChangeState;

		public Cell(int x, int y, bool alive)
		{
			Position = new Vector2(x, y);
			IsAlive = alive;
		}
		public Cell(Vector2 position, bool alive, bool isRemoveCandidate)
		{
			Position = position;
			IsAlive = alive;
			IsRemoveCandidate = isRemoveCandidate;
		}
	}

	public class CellEqualityComparer : IEqualityComparer<Cell>
	{
		public int GetHashCode(Cell obj) => 0;
		public bool Equals(Cell a, Cell b) => a.Position.Equals(b.Position);
	}

	class CellComparer : IComparer<Cell>
	{
		public int Compare(Cell a, Cell b)
		{
			if (a.Position.X == b.Position.X && a.Position.Y == b.Position.Y)
				return 0;
			if (a.Position.X < b.Position.X || (a.Position.X == b.Position.X && a.Position.Y < b.Position.Y))
				return -1;
			return 1;
		}
	}

}
