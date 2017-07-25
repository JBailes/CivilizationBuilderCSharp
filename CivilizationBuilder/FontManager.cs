using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CivilizationBuilder
{
	static class FontManager
	{
		static ContentManager contentManager;
		static Dictionary<string, SpriteFont> fontList = new Dictionary<string, SpriteFont>();

		public static SpriteFont GetFont(string name)
		{
			if (!fontList.ContainsKey(name))
			{
				fontList.Add(name, contentManager.Load<SpriteFont>(@"Fonts/" + name));
			}
			
			return fontList[name];
		}

		public static void SetContentManager(ContentManager content)
		{
			contentManager = content;
		}
	}
}
