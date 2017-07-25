using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilizationBuilder
{
	public abstract class Actor : Component
	{
		public long state;
		protected string name;
		protected long endurance;
		protected long strength;
		protected long intelligence;
	}
}
