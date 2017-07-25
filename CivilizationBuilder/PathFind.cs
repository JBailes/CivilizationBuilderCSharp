using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilizationBuilder
{
	class PathFind
	{
		static Dictionary<Tuple<int, int, int, int>, Path> paths = new Dictionary<Tuple<int, int, int, int>, Path>();

		public static Path memoizedPaths(Point origin, Point target, long heuristic = -1)
		{
			var key = new Tuple <int, int, int, int>(origin.X, origin.Y, target.X, target.Y);
			if (paths.ContainsKey(key))
				return paths[key];

			var path = GeneratePath(origin, target, heuristic);

			if (path == null)
				return null;

			paths.Add(key, path);

			return paths[key];
		}

		public static void InvalidatePath(Point origin, Point target)
		{
			paths.Remove(new Tuple<int, int, int, int>(origin.X, origin.Y, target.X, target.Y));
		}

		public static Path FastGeneratePath(Point origin, Point target)
		{
			// It's actually faster to do this twice, once with an extremely small heuristic applied,
			// then again with no upper limit. This way we either path really fast (99% of the time),
			// or we find if any possible path exists.
			long heuristic = Math.Abs(target.X - origin.X) + Math.Abs(target.Y - origin.Y);

			var path = memoizedPaths(origin, target, heuristic*2);

			if (path == null)
				path = memoizedPaths(origin, target);

			return path;
		}

		public static Path SlowGeneratePath(Point origin, Point target)
		{
			return memoizedPaths(origin, target);
		}

		private static Path GeneratePath(Point origin, Point target, long heuristic = -1)
		{
			if (heuristic == -1)
				heuristic = long.MaxValue;

			long foundCost = long.MaxValue;

			int movementCost = 2;

			PriorityQueue<PathTile> queue = new PriorityQueue<PathTile>();

			Dictionary<Point, PathTile> tiles = new Dictionary<Point, PathTile>();

			tiles.Add(target, new PathTile(target.X, target.Y, 2));

			queue.Enqueue(tiles[target], 1);

			while(queue.HasNext())
			{
				var current = queue.Dequeue();

				for(int x = -1; x <= 1; x++)
				{
					for(int y = -1; y <= 1; y++)
					{
						if (y == 0 && x == 0)
							continue;

						if (current.X + x < 0 || current.Y + y < 0 || current.X + x >= World.worldSize || current.Y + y >= World.worldSize)
							continue;

						if (World.world[current.X + x, current.Y + y] == World.TILE_IMPASSABLE)
							continue;

						int cost = movementCost;

						if (y != 0 && x != 0)
							cost = 3;

						cost += current.cost;

						if (cost > heuristic || cost >= foundCost)
							continue;

						var point = new Point(current.X + x, current.Y + y);
						
						if (!tiles.ContainsKey(point))
						{
							tiles.Add(point, new PathTile(current.X + x, current.Y + y, cost));
							queue.Enqueue(tiles[point], tiles[point].cost);
						}
						else
						{
							if (origin.X == point.X && origin.Y == point.Y)
							{
								foundCost = cost;
								heuristic = cost;
							}

							if (tiles[point].cost > cost)
							{
								tiles[point].cost = cost;
								queue.Enqueue(tiles[point], tiles[point].cost);
							}
						}
					}
				}
			}

			return new Path(Backtrack(tiles, origin, target), origin, target);
		}

		static List<Point> Backtrack(Dictionary<Point, PathTile> tiles, Point origin, Point target)
		{
			List<Point> generatedPath = new List<Point>();

			int x = origin.X;
			int y = origin.Y;
			int previ = 0;
			int prevj = 0;

			int count = 0;

			bool newDir = false;

			while (!inRange(target, x, y))
			{
				count++;
				int lowcost = int.MaxValue;
				int lowx = 0, lowy = 0;

				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						if (i == 0 && j == 0)
							continue;
						
						Point cur = new Point(x + i, y + j);

						if (tiles.ContainsKey(cur) && lowcost > tiles[cur].cost)
						{
							if (World.world[x + i, y + j] == World.TILE_IMPASSABLE)
							{
								tiles.Remove(cur);
								continue;
							}

							lowx = x + i;
							lowy = y + j;

							if (i == previ && j == prevj)
								newDir = false;
							else
								newDir = true;

							lowcost = tiles[cur].cost;
						}
					}
				}

				previ = x-lowx;
				prevj = y-lowy;
				
				x = lowx;
				y = lowy;

				tiles.Remove(new Point(x, y));
				
				generatedPath.Add(new Point(x, y));
			}

			generatedPath.Add(target);

			return generatedPath;
		}

		private static bool inRange(Point target, int x, int y)
		{
			if (x > target.X + 1)
				return false;

			if (x < target.X - 1)
				return false;

			if (y > target.Y + 1)
				return false;

			if (y < target.Y - 1)
				return false;

			return true;
		}

		private static int GetMovementCost(int x, int y)
		{
			if (World.world[x, y] == World.TILE_WATER)
				return 1000;

			return 1;
		}
	}
}
