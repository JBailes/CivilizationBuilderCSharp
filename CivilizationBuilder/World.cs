using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CivilizationBuilder
{
	public static class World
	{
		static int ticksPerSecond = 60;
		static int msPerTick = 1000 / ticksPerSecond;
		static int worldUpdateTime = 1000;

		static int villagerCount = 100000;

		public static int worldSize = 21;

		public static int[,] world;

		public static List<Villager> villagers = new List<Villager>();

		static Thread worldUpdateThread;
		static Thread tickThread;

		public const int TILE_WATER = 18;
		public const int TILE_IMPASSABLE = 3;
		public const int TILE_FOOD = 4;
		public const int TILE_WORK = 7;
		public const int TILE_REST = 10;
	

		public static void WorldThreadCreation()
		{
			world = new int[worldSize, worldSize];

			for(int i = 0; i < worldSize; i++)
			{
				for(int j = 0; j < worldSize; j++)
				{
					if (i == 19 && j == 19)
						world[i, j] = TILE_FOOD;
					else if (i == 1 && j == 19)
						world[i, j] = TILE_WORK;
					else if (i == 1 && j == 1)
						world[i, j] = TILE_REST;
					else if (i == 19 && j == 1)
						world[i, j] = TILE_WATER;
					else if (i >= 7 && i <= 15 && j == 5)
						world[i, j] = TILE_IMPASSABLE;
					else if (j >= 7 && j <= 15 && i == 5)
						world[i, j] = TILE_IMPASSABLE;
					else
						world[i, j] = 1;
				}
			}

			for (int i = 0; i < villagerCount; i++)
				villagers.Add(new Villager("Villager " + i));

			worldUpdateThread = new Thread(worldUpdate);

			worldUpdateThread.Name = "World Update Thread";

			worldUpdateThread.IsBackground = true;

			worldUpdateThread.Start();

			tickThread = new Thread(tick);

			tickThread.Name = "World Tick Thread";

			tickThread.IsBackground = true;

			tickThread.Start();
		}

		public static void WorldExit()
		{
			try
			{
				worldUpdateThread.Abort();
				tickThread.Abort();
			}
			catch (Exception) { }
		}

		private static void tick()
		{
			Stopwatch sw = new Stopwatch();

			sw.Start();

			long timelapse = msPerTick;

			while (true)
			{
				long cur = sw.ElapsedMilliseconds;

				if (timelapse < msPerTick)
					timelapse = msPerTick;

				foreach (var villager in villagers)
					villager.tick(timelapse);

				timelapse = sw.ElapsedMilliseconds - cur;

				if (timelapse < msPerTick)
					Thread.Sleep(msPerTick - (int)timelapse);
			}
		}

		private static void worldUpdate()
		{
			Stopwatch sw = new Stopwatch();

			sw.Start();

			long cur, time;

			while (true)
			{
				cur = sw.ElapsedMilliseconds;

				foreach (var villager in villagers)
					villager.update();

				time = sw.ElapsedMilliseconds - cur;

				if (time < worldUpdateTime)
					Thread.Sleep(worldUpdateTime - (int)time);

			}
		}
	}
}
