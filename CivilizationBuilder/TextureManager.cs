using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CivilizationBuilder
{
	class TextureManager
	{
		static ContentManager contentManager;
		static Dictionary<string, Texture> textureList = new Dictionary<string, Texture>();

		public static Texture GetTexture(string name, int tileWidth = -1, int tileHeight = -1)
		{
			if (!textureList.ContainsKey(name))
			{
				var temp = contentManager.Load<Texture2D>(@"Images/" + name);

				if (tileWidth == -1)
					tileWidth = temp.Width;

				if (tileHeight == -1)
					tileHeight = temp.Height;

				textureList.Add(name, new Texture(temp, tileWidth, tileHeight));
			}

			return textureList[name];
		}

		public static void SetContentManager(ContentManager content)
		{
			contentManager = content;
		}
	}
}
