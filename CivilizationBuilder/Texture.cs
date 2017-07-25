using Microsoft.Xna.Framework.Graphics;

namespace CivilizationBuilder
{
	public class Texture
	{
		public Texture2D texture { get; private set; }
		public int tileHeight { get; private set; }
		public int tileWidth { get; private set; }

		public Texture(Texture2D texture, int tileWidth, int tileHeight)
		{
			this.texture = texture;
			this.tileWidth = tileWidth;
			this.tileHeight = tileHeight;
		}
	}
}
