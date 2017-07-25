using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilizationBuilder
{
	class Path
	{
		public List<Point> curPath;
		public Point start;
		public Point end;
		public long timeStamp;
		public int cost;

		public Path(List<Point> curPath, Point start, Point end)
		{
			this.curPath = curPath;
			this.start = start;
			this.end = end;
			timeStamp = DateTime.Now.Millisecond;
		}

		public Path(List<Point> newPath, List<Point> oldPath, Point start, Point end)
		{
			curPath = newPath;
			curPath.AddRange(oldPath);
			this.start = start;
			this.end = end;
			timeStamp = DateTime.Now.Millisecond;
		}

		public void SetCost(int cost)
		{
			this.cost = cost;
		}

		public int GetCost()
		{
			return cost;
		}
	}
}
