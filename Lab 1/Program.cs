using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PortableConsole
{
	public class Program
	{
		public static bool[,] Read(string file)
		{
			try
			{
				var lines = File.ReadAllLines(file).ToList();
				lines.RemoveAll(line => line == "");
				var matrix = lines.Select(line => line.ToCharArray()).ToArray();
				var count = lines.Count;
				var length = lines.Max(line => line.Length);
				bool[,] cells = new bool[length + 2, count + 2];
				for (int x = 0; x < length; x++)
					for (int y = 0; y < count; y++)
						switch (matrix[y][x])
						{
							case '1':
								cells[x + 1, y + 1] = true;
								break;
							case '0':
								cells[x + 1, y + 1] = false;
								break;
							default:
								throw new Exception("Input contains wrong symbols");
						}
				return cells;
			}
			catch (Exception e)
			{
				if (e is IndexOutOfRangeException)
					Console.WriteLine("Input is in wrong format");
				else
					Console.WriteLine(e.Message);
				return null;
			}
		}

		public static Stream Open(string file)
		{
			try
			{
				return File.Create(file);
			}
			catch
			{
				return Console.OpenStandardOutput();
			}

		}

		public static bool HandleArgs(string[] args, ref bool[,] input, ref Stream output, ref int steps)
		{
			steps = 0;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == "-i" || args[i] == "--input")
				{
					if (i + 1 == args.Length || args[i + 1].StartsWith("-"))
					{
						Console.WriteLine("Input file argument not found");
						return false;
					}
					else
						input = Read(args[i + 1]);
				}
				if (args[i] == "-o" || args[i] == "--output")
				{
					if (i + 1 == args.Length || args[i + 1].StartsWith("-"))
					{
						Console.WriteLine("Output file argument not found");
						return false;
					}
					else
						output = Open(args[i + 1]);
				}
				if (args[i] == "-n" || args[i] == "--number-of-iterations")
				{
					if (i + 1 == args.Length || args[i + 1].StartsWith("-"))
					{
						Console.WriteLine("Number of iterations argument not found");
						return false;
					}
					else
						Int32.TryParse(args[i + 1], out steps);
				}
			}

			return true;
		}

		public static void Main(string[] args)
		{
			bool[,] input = null;
			Stream output = Console.OpenStandardOutput();
			int steps = 0;

			if (!HandleArgs(args, ref input, ref output, ref steps))
				return;

			if (input == null)
			{
				Console.WriteLine("Missing Input");
				return;
			}

			Conway conway = new Conway();

			var cells = new List<Cell>();

			for (int i = 0; i < input.GetLength(0); i++)
			{
				for (int j = 0; j < input.GetLength(1); j++)
				{
					cells.Add(new Cell(i, j, input[i, j]));
				}
			}
			conway.Initialize(cells);
			
			for (int i = 0; i < steps; i++)
				conway.Step();
			
			var result = conway.Output();

			output.Write(result.ToArray(), 0, result.Count);
			output.Flush();

		}
	}
}
