using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Slipscream
{
	public class SlipscreamGame : Game
	{
		private int bendFactor;
		private int position;
		private Texture2D? road;
		private SpriteBatch? spriteBatch;

		int Height => GraphicsDevice.PresentationParameters.BackBufferHeight;

		public SlipscreamGame()
		{
			_ = new GraphicsDeviceManager(this);
			IsFixedTimeStep = false;
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			spriteBatch!.Begin(blendState: BlendState.NonPremultiplied);

			for (int z = 0; z < Height; z++)
			{
				int y = Height - z - 1;
				int x = z / 2 + (int) (Math.Sqrt(1 - Math.Pow((float) z / Height, 2)) * bendFactor) + 100 - bendFactor;
				spriteBatch.Draw(road, new Rectangle(x, y, road!.Width - z, 1), new Rectangle(0, (y - position + Height) % Height, road.Width, 1), Color.White);
			}

			spriteBatch.End();
			base.Draw(gameTime);
		}
		
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			road = Texture2D.FromFile(GraphicsDevice, "Road.png");
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			bendFactor = (int) (Math.Sin((int) gameTime.TotalGameTime.TotalMilliseconds % 2_000 / 2_000.0 * Math.PI * 2) * 300);
			position = (int) gameTime.TotalGameTime.TotalMilliseconds % Height;
			base.Update(gameTime);
		}
	}
}