using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Slipscream
{
	public class Font
	{
		private const string Filename = @"Assets\Font.png";

		private const int CharacterWidth = 20;
		private const int CharacterHeight = 20;
		private const int CharactersPerRow = 15;

		private readonly Texture2D texture;

		public Font(GraphicsDevice device)
		{
			texture = Texture2D.FromFile(device, Filename);
		}

		public void Write(SpriteBatch spriteBatch, int x, int y, string text)
		{
			foreach (char character in text.ToUpper().Select(character => character is >= ' ' and <= 'Z' ? character : '?'))
			{
				int index = character - ' ';
				spriteBatch.Draw(texture, new Rectangle(x, y, CharacterWidth, CharacterHeight),
					new Rectangle(index % CharactersPerRow * CharacterWidth, index / CharactersPerRow * CharacterHeight, CharacterWidth, CharacterHeight), Color.White);
				x += CharacterWidth;
			}
		}
	}
}