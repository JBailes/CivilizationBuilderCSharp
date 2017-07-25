using System;
using System.Collections.Generic;

namespace CivilizationBuilder
{
    public static class Debug
    {
		private static int debug = 6;
		private static List<string> message = new List<string>();
		public static string worldTick = "";
		public static string worldUpdate = "";

		private static Random rand = new Random();

		public static void Print(string line, int debugLevel = 9)
		{
			if (debugLevel <= debug)
				message.Add(line);
		}

		public static List<string> GetMessages()
		{
			return message;
		}

		public static int GetRandomNumber(int min, int max)
		{
			return rand.Next(min, max);
		}
    }
}
