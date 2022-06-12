using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slipscream
{
	public class Dot
	{
		private readonly Texture2D texture;

		public Dot(GraphicsDevice device)
		{
			texture = new Texture2D(device, 1, 1);
			Color[] colors = new Color[] { Color.White };
			texture.SetData(colors);
		}

		public void Draw(SpriteBatch spriteBatch, int x, int y, int width, int height, Color color) => spriteBatch.Draw(texture, new Rectangle(x, y, width, height), null, color);
	}
}