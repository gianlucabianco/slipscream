using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slipscream
{
	public class SlipscreamGame : Game
	{
		private Texture2D? car;
		private Vector2 position;
		private SpriteBatch? spriteBatch;

		public SlipscreamGame() => _ = new GraphicsDeviceManager(this);

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			spriteBatch!.Begin();
			spriteBatch.Draw(car, position, Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			car = Texture2D.FromFile(GraphicsDevice, "Car.png");
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			position.X = (int) gameTime.TotalGameTime.TotalMilliseconds % GraphicsDevice.PresentationParameters.BackBufferWidth;
			position.Y = (int) gameTime.TotalGameTime.TotalMilliseconds % GraphicsDevice.PresentationParameters.BackBufferHeight;

			base.Update(gameTime);
		}
	}
}