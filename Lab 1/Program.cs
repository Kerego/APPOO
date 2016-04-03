using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PortableConsole
{
	public struct Vector2
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Vector2(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public class CellComparer : IEqualityComparer<Cell>
	{
		public int GetHashCode(Cell obj) => 0;
		public bool Equals(Cell a, Cell b) => a.Position.Equals(b.Position);
	}

	public class Cell
	{
		public Vector2 Position;
		public bool IsAlive;
		public bool IsRemoveCandidate = true;
		public bool IsBornCandidate;
		public bool NotChangeState;
		public Cell(int x, int y)
		{
			Position = new Vector2(x, y);
		}
		public Cell(Vector2 position)
		{
			Position = position;
		}
		public Cell(Vector2 position, bool alive)
		{
			Position = position;
			IsAlive = alive;
		}
		public Cell(int x, int y, bool alive)
		{
			Position = new Vector2(x, y);
			IsAlive = alive;
		}

		public Cell(int x, int y, bool alive, bool remove)
		{
			Position = new Vector2(x, y);
			IsAlive = alive;
			IsRemoveCandidate = remove;
		}
		public Cell(Vector2 position, bool alive, bool isRemoveCandidate)
		{
			Position = position;
			IsAlive = alive;
			IsRemoveCandidate = isRemoveCandidate;
		}

		public Cell()
		{

		}

		//public Cell[] Neighbours() => new Cell[8]
		//{
		//	new Cell(this.Position.X - 1, this.Position.Y - 1, false), new Cell(this.Position.X, this.Position.Y - 1, false),   new Cell(this.Position.X + 1, this.Position.Y - 1, false),
		//	new Cell(this.Position.X - 1, this.Position.Y, false),                                        new Cell(this.Position.X + 1, this.Position.Y, false),
		//	new Cell(this.Position.X - 1, this.Position.Y + 1, false), new Cell(this.Position.X, this.Position.Y + 1, false),   new Cell(this.Position.X + 1, this.Position.Y + 1, false),
		//};
	}

	public class Conway
	{

		public List<List<Cell>> Map = new List<List<Cell>>();
		CellComparer _comparer = new CellComparer();

		public void Initialize(List<Cell> cells)
		{
			Map = Group(cells);
		}


		public void ChangeCellState(Vector2 position)
		{
			var list = Map.SelectMany(x => x).ToList();
			bool flag = false;
			int index = 0;
			var neighbours = new List<Vector2>
			{
				new Vector2(position.X - 1, position.Y - 1), new Vector2(position.X, position.Y - 1),   new Vector2(position.X + 1, position.Y - 1),
				new Vector2(position.X - 1, position.Y),                                                new Vector2(position.X + 1, position.Y),
				new Vector2(position.X - 1, position.Y + 1), new Vector2(position.X, position.Y + 1),   new Vector2(position.X + 1, position.Y + 1),
			};
			var existing = new List<Vector2>();

			for (int i = 0; i < list.Count; i++)
			{
				if (neighbours.Contains(list[i].Position))
					existing.Add(list[i].Position);
				if (list[i].Position.Equals(position))
				{
					list[i].IsAlive = !list[i].IsAlive;
					flag = true;
					index = i;
				}
			}

			foreach (var n in neighbours.Except(existing))
			{
				list.Add(new Cell(n, false));
			}

			if (!flag)
			{
				list.Add(new Cell(position, true));
			}
			Initialize(list);
		}


		public List<List<Cell>> GetCurrentState(int xLimit, int yLimit)
		{
			return Map
				.Where(group => group[0].Position.X < xLimit)
				.Select(group => group.Where(cell => cell.Position.Y < yLimit).ToList())
				.ToList();
		}

		private List<List<Cell>> Group(List<Cell> toGroup)
		{
			var map = new List<List<Cell>>();
			var sorted = toGroup.OrderBy(cell => cell.Position.X).ThenBy(cell => cell.Position.Y).ToList();
			if (sorted.Count == 0)
				return map;

			int currentKey = sorted[0].Position.X;
			int mapIndex = 0;
			map.Add(new List<Cell>());
			for (int i = 0; i < sorted.Count; i++)
			{

				if (sorted[i].Position.X == currentKey)
				{
					map[mapIndex].Add(sorted[i]);
				}
				else
				{
					map.Add(new List<Cell>());
					currentKey = sorted[i].Position.X;

					mapIndex++;
					map[mapIndex].Add(sorted[i]);
				}
			}
			return map;
		}


		private object _lockObject = 0;

		public void Step()
		{
			List<List<Cell>> next = Map.Select(x => x.ToList()).ToList();
			List<Cell> outsideNeighbours = new List<Cell>();
			List<List<Cell>> insideNeighbours = Map.Select(x => new List<Cell>()).ToList();

			//Parallel.For(0, Map.Count, currentX =>
			for (int currentX = 0; currentX < Map.Count; currentX++)
			{
				int tempYl = 0;
				int tempYm = 0;
				int tempYr = 0;
				for (int currentY = 0; currentY < Map[currentX].Count; currentY++)
				{
					int count = 0;
					var currentCellPosition = Map[currentX][currentY].Position;
					Cell[] neighbours = new Cell[9];

					//left
					if (currentX != 0)
					{
						if (Math.Abs(Map[currentX - 1][0].Position.X - currentCellPosition.X) < 2)
						{
							while (Map[currentX - 1].Count > tempYl && Map[currentX - 1][tempYl].Position.Y < currentCellPosition.Y - 1)
								tempYl++;

							while (Map[currentX - 1].Count > tempYl && Map[currentX - 1][tempYl].Position.Y < currentCellPosition.Y + 2)
							{
								if (Map[currentX - 1][tempYl].Position.Y == currentCellPosition.Y - 1)
									neighbours[0] = Map[currentX - 1][tempYl];
								else if (Map[currentX - 1][tempYl].Position.Y == currentCellPosition.Y)
									neighbours[3] = Map[currentX - 1][tempYl];
								else if (Map[currentX - 1][tempYl].Position.Y == currentCellPosition.Y + 1)
									neighbours[6] = Map[currentX - 1][tempYl];
								tempYl++;
							}
						}
					}

					//middle
					while (Map[currentX].Count > tempYm && Map[currentX][tempYm].Position.Y < currentCellPosition.Y - 1)
						tempYm++;

					while (Map[currentX].Count > tempYm && Map[currentX][tempYm].Position.Y < currentCellPosition.Y + 2)
					{
						if (Map[currentX][tempYm].Position.Y == currentCellPosition.Y - 1)
							neighbours[1] = Map[currentX][tempYm];
						else if (Map[currentX][tempYm].Position.Y == currentCellPosition.Y + 1)
							neighbours[7] = Map[currentX][tempYm];
						tempYm++;
					}

					//right
					if (currentX != Map.Count - 1)
					{
						if (Math.Abs(Map[currentX + 1][0].Position.X - currentCellPosition.X) < 2)
						{
							while (Map[currentX + 1].Count > tempYr && Map[currentX + 1][tempYr].Position.Y < currentCellPosition.Y - 1)
								tempYr++;

							while (Map[currentX + 1].Count > tempYr && Map[currentX + 1][tempYr].Position.Y < currentCellPosition.Y + 2)
							{
								if (Map[currentX + 1][tempYr].Position.Y == currentCellPosition.Y - 1)
									neighbours[2] = Map[currentX + 1][tempYr];
								else if (Map[currentX + 1][tempYr].Position.Y == currentCellPosition.Y)
									neighbours[5] = Map[currentX + 1][tempYr];
								else if (Map[currentX + 1][tempYr].Position.Y == currentCellPosition.Y + 1)
									neighbours[8] = Map[currentX + 1][tempYr];
								tempYr++;
							}
						}
					}

					count = neighbours.Count(neighbour => neighbour != null && neighbour.IsAlive);

					var cell = Map[currentX][currentY];
					if (count == 3 || (count == 2 && cell.IsAlive))
					{
						lock (_lockObject)
						{
							cell.IsBornCandidate = true;
							FindMissingNeighbours(currentX, outsideNeighbours, insideNeighbours, currentCellPosition, neighbours);
						}
					}
					else
					{
						cell.IsRemoveCandidate = count == 0;
						cell.IsBornCandidate = false;
					}


					if (tempYl != 0)
						tempYl = tempYl > 3 ? tempYl - 3 : 0;
					if (tempYm != 0)
						tempYm = tempYm > 3 ? tempYm - 3 : 0;
					if (tempYr != 0)
						tempYr = tempYr > 3 ? tempYr - 3 : 0;

				}
			}


			next.ForEach(group => group.ForEach(cell =>
			{
				if (cell.IsBornCandidate)
				{
					cell.IsAlive = true;
					cell.IsRemoveCandidate = false;
				}
				else
				{
					cell.IsAlive = false;
				}
			}));

			next.ForEach(group => group.RemoveAll(cell => cell.IsRemoveCandidate && !cell.NotChangeState));


			for (int i = 0; i < next.Count; i++)
			{
				next[i].AddRange(insideNeighbours[i].Distinct(_comparer));
				next[i] = next[i].OrderBy(cell => cell.Position.Y).ToList();
			}



			next.RemoveAll(group => group.Count == 0);




			var outside = Group(outsideNeighbours.Distinct(_comparer).ToList());
			AddNeighbours(outside, next);



			next.ForEach(group => group.ForEach(cell => { cell.IsRemoveCandidate = true; cell.NotChangeState = false; }));



			Map = next;

		}

		private void FindMissingNeighbours(int currentX, List<Cell> outsideNeighbours, List<List<Cell>> insideNeighbours, Vector2 currentCellPosition, Cell[] neighbours)
		{
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					var index = (i + 1) * 3 + (j + 1);
					if (i == 0 && j == 0)
						continue;
					if (neighbours[index] == null)
					{
						if (currentX + j == -1 || currentX + j == Map.Count || Map[currentX + j][0].Position.X != currentCellPosition.X + j)
							outsideNeighbours.Add(new Cell(new Vector2(currentCellPosition.X + j, currentCellPosition.Y + i), false, false));
						else
							insideNeighbours[currentX + j].Add(new Cell(new Vector2(currentCellPosition.X + j, currentCellPosition.Y + i), false, false));

					}
					else
						neighbours[index].NotChangeState = true;
				}
			}
		}


		private void AddNeighbours(List<List<Cell>> sortedNeighbours, List<List<Cell>> map)
		{
			for (int i = 0; i < sortedNeighbours.Count; i++)
			{
				bool existFlag = false;
				for (int j = 0; j < map.Count; j++)
				{
					if (sortedNeighbours[i][0].Position.X == map[j][0].Position.X)
					{
						existFlag = true;
						map[j].AddRange(sortedNeighbours[i].Except(map[j], _comparer));
						break;
					}
				}
				if (!existFlag)
				{
					var x = sortedNeighbours[i][0].Position.X;
					if (map.Count == 0 || x < map[0][0].Position.X)
						InsertNewGroup(sortedNeighbours[i], map, 0);
					else if (x > map[map.Count - 1][0].Position.X)
						InsertNewGroup(sortedNeighbours[i], map, map.Count);
					else
						for (int groupIndex = 1; groupIndex < map.Count - 2; groupIndex++)
						{
							if (map[groupIndex][0].Position.X < x && map[groupIndex + 1][0].Position.X > x)
							{
								InsertNewGroup(sortedNeighbours[i], map, groupIndex + 1);
							}
						}
				}
			}
		}

		private void InsertNewGroup(List<Cell> neighboursGroup, List<List<Cell>> map, int groupIndex)
		{
			map.Insert(groupIndex, neighboursGroup);
		}

		public string Output(List<List<Cell>> list)
		{
			var result = "";
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list[i].Count; j++)
				{
					result += list[i][j].Position.X + (list[i][j].IsAlive ? "*" : "_") + list[i][j].Position.Y + "  ";
				}
				result += Environment.NewLine;
			}
			return result;
		}

	}


	public class Program
    {
        public static void Main(string[] args)
        {
			Conway conway = new Conway();
			var rand = new Random();
			var list = new List<Cell>();

			//for (int i = 0; i < 50; i++)
			//{
			//	for (int j = 0; j < 50; j++)
			//	{
			//		list.Add(new Cell() { Position = new Vector2(i, j), IsAlive = rand.NextDouble() > 0.5 });
			//	}
			//}

			list.Add(new Cell(2, 3, false));
			list.Add(new Cell(2, 4, false));
			list.Add(new Cell(2, 5, false));
			list.Add(new Cell(2, 6, false));
			list.Add(new Cell(2, 7, false));


			list.Add(new Cell(3, 3, false));
			list.Add(new Cell(3, 4, true));
			list.Add(new Cell(3, 5, true));
			list.Add(new Cell(3, 6, true));
			list.Add(new Cell(3, 7, false));

			list.Add(new Cell(4, 3, false));
			list.Add(new Cell(4, 4, false));
			list.Add(new Cell(4, 5, false));
			list.Add(new Cell(4, 6, false));
			list.Add(new Cell(4, 7, false));




			Stopwatch sw = Stopwatch.StartNew();
			conway.Initialize(list);
			for (int i = 0; i < 1000000; i++)
			{
				conway.Step();
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
			for (int i = 0; i < conway.Map.Count; i++)
			{
				for (int j = 0; j < conway.Map[i].Count; j++)
				{
					Console.Write(conway.Map[i][j].IsAlive ? "*" : "+");
				}
				Console.WriteLine("");
			}

			Console.WriteLine($"Done ");
			Console.Read();
		}
    }
}
