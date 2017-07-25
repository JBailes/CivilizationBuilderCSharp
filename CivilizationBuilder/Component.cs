using Microsoft.Xna.Framework;

namespace CivilizationBuilder
{
	public abstract class Component
	{
		public int x, y, height, width;
		public Rectangle size()
		{
			return new Rectangle(x, y, width, height);
		}
	}
}
