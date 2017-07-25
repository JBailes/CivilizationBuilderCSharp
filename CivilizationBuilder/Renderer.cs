using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CivilizationBuilder
{
	public class Renderer
	{
		GraphicsDeviceManager graphicsManager;
		SpriteBatch spriteBatch;
		SpriteBatch absoluteSpriteBatch;
		Camera testc;

		public static double scaling = 1.0;

		public Renderer(GraphicsDeviceManager graphicsManager, SpriteBatch spriteBatch, SpriteBatch absoluteSpriteBatch)
		{
			this.graphicsManager = graphicsManager;

			this.spriteBatch = spriteBatch;

			this.absoluteSpriteBatch = absoluteSpriteBatch;

			testc = new Camera(graphicsManager.GraphicsDevice.Viewport);
		}

		public void StartFrame()
		{
			testc.UpdateCamera(graphicsManager.GraphicsDevice.Viewport);
			spriteBatch.Begin(SpriteSortMode.Deferred,
						BlendState.AlphaBlend,
						SamplerState.PointWrap,
						null,
						null,
						null,
						testc.Transform);

			absoluteSpriteBatch.Begin();
			graphicsManager.GraphicsDevice.Clear(Color.DarkMagenta);
		}

		public void RenderWorld()
		{
			Texture tile = TextureManager.GetTexture("terrain", 32, 32);

			int leftBound = testc.VisibleArea.Left / tile.tileWidth - 1;
			int rightBound = testc.VisibleArea.Right / tile.tileWidth + 1;
			int topBound = testc.VisibleArea.Top / tile.tileHeight - 1;
			int bottomBound = testc.VisibleArea.Bottom / tile.tileHeight + 1;

			for (int i = leftBound; i <= rightBound; i++)
			{
				for(int j = topBound; j <= bottomBound; j++)
				{
					if (i >= 0 && j >= 0 && i < World.worldSize && j < World.worldSize)
						spriteBatch.Draw(tile.texture, new Rectangle(i * tile.tileWidth, j * tile.tileHeight, tile.tileWidth, tile.tileHeight), new Rectangle(World.world[i,j]*tile.tileWidth, 5*tile.tileHeight, tile.tileWidth, tile.tileHeight), Color.White);
				}
			}

			foreach(var villager in World.villagers)
			{
				if (villager.x >= testc.VisibleArea.Left && villager.x <= testc.VisibleArea.Right && villager.y >= testc.VisibleArea.Top && villager.y <= testc.VisibleArea.Bottom)
				{
					spriteBatch.Draw(TextureManager.GetTexture(villager.sprite).texture, new Rectangle(villager.x - Villager.xOffset, villager.y - Villager.yOffset, 50, 50), new Rectangle(Villager.tileWidth * villager.xFrame, Villager.tileHeight * villager.yFrame, Villager.tileWidth, Villager.tileHeight), Color.White);
				}
			}
		}

		public void EndFrame()
		{
			spriteBatch.End();
			absoluteSpriteBatch.End();
		}
		
		public void RenderFont(string font, string value, int x, int y, Color color)
		{
			absoluteSpriteBatch.DrawString(FontManager.GetFont(font), value, new Vector2(x, y), color);
		}

		public void RenderFullScreen(string texture)
		{
			absoluteSpriteBatch.Draw(TextureManager.GetTexture(texture).texture, Vector2.Zero, Color.White);
		}

		public void RenderImage(string texture, Rectangle destRect, Rectangle srcRect)
		{
			absoluteSpriteBatch.Draw(TextureManager.GetTexture(texture, srcRect.Width, srcRect.Height).texture, destRect, srcRect, Color.White);
		}

		public void SetResolution(int width, int height)
		{
			graphicsManager.PreferredBackBufferHeight = height;
			graphicsManager.PreferredBackBufferWidth = width;
		}

		public void RemoveVsync()
		{
			graphicsManager.SynchronizeWithVerticalRetrace = false;
			graphicsManager.ApplyChanges();
		}
	}
}
