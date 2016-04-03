using System;
using System.Collections.Generic;
using System.Linq;

namespace PortableConsole
{
	public class Conway
	{
		public List<List<Cell>> Map = new List<List<Cell>>();
		CellComparer _comparer = new CellComparer();
		CellSortComparer _sortComparer = new CellSortComparer();

		public void Initialize(List<Cell> cells)
		{
			Map = Group(cells);
		}

		public List<byte> Output()
		{
			if (Map.Count == 0)
				return new List<byte>();
			int left = int.MaxValue;
			int right = int.MinValue;
			int top = int.MaxValue;
			int bottom = int.MinValue;
			foreach (var group in Map)
			{
				foreach (var item in group)
				{
					if (!item.IsAlive)
						continue;
					if (item.Position.Y < top)
						top = item.Position.Y;
					if (item.Position.Y > bottom)
						bottom = item.Position.Y;
					if (item.Position.X < left)
						left = group[0].Position.X;
					if (item.Position.X > right)
						right = group[0].Position.X;
				}
			}

			int[] counters = new int[Map.Count];

			var dictionaries = Map.ToDictionary(x => x[0].Position.X, l => l.ToDictionary(x => x.Position.Y));
			
			List<byte> res = new List<byte>();

			for (int y = top; y < bottom + 1; y++)
			{
				for (int x = left; x < right + 1; x++)
				{
					if (!dictionaries.ContainsKey(x) || !dictionaries[x].ContainsKey(y))
					{
						res.Add(0x30);
						continue;
					}
					res.Add(dictionaries[x][y].IsAlive ? (byte)0x31 : (byte)0x30);
				}
                res.AddRange(Environment.NewLine.Select(ch => (byte)ch));
			}


			return res;
		}

		public void Step()
		{
			List<List<Cell>> next = new List<List<Cell>>();
			List<List<Cell>> insideNeighbours = new List<List<Cell>>();
			List<Cell> outsideNeighbours = new List<Cell>();
			for (int i = 0; i < Map.Count; i++)
			{
				next.Add(new List<Cell>(Map[i]));
				insideNeighbours.Add(new List<Cell>());
			}

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

					foreach (var neighbour in neighbours)
					{
						if (neighbour != null && neighbour.IsAlive)
							count++;
					}

					var cell = Map[currentX][currentY];
					if (count == 3 || (count == 2 && cell.IsAlive))
					{
						cell.IsBornCandidate = true;
						FindMissingNeighbours(currentX, outsideNeighbours, insideNeighbours, currentCellPosition, neighbours);
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
				next[i].Sort(_sortComparer);
			}



			next.RemoveAll(group => group.Count == 0);




			var outside = Group(outsideNeighbours.Distinct(_comparer).ToList());
			AddNeighbours(outside, next);



			next.ForEach(group => group.ForEach(cell => { cell.IsRemoveCandidate = true; cell.NotChangeState = false; }));



			Map = next;

		}

		private List<List<Cell>> Group(List<Cell> toGroup)
		{
			var map = new List<List<Cell>>();
			//var sorted = toGroup.OrderBy(cell => cell.Position.X).ThenBy(cell => cell.Position.Y).ToList();
			toGroup.Sort(_sortComparer);
			var sorted = toGroup;
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
		
	}

}
