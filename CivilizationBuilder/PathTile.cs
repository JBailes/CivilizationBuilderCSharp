using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilizationBuilder
{
	class PathTile
	{
		public int cost;
		public int X;
		public int Y;

		public PathTile(int X, int Y, int cost)
		{
			this.X = X;
			this.Y = Y;
			this.cost = cost;
		}
	}
}
