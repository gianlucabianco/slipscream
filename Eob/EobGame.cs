using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Eob
{
	public class EobGame : Game
	{
		private SpriteBatch? spriteBatch;
		private Texture2D? wall;

		private int Height => GraphicsDevice.PresentationParameters.BackBufferHeight;
		private const double ViewingDistance = 0.5;
		private int Width => GraphicsDevice.PresentationParameters.BackBufferWidth;

		public EobGame()
		{
			_ = new GraphicsDeviceManager(this);
			IsFixedTimeStep = false;
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			spriteBatch!.Begin(blendState: BlendState.NonPremultiplied);

			for (int x = -1; x <= 1; x++)
			{
				const double TargetZ = 1.5;

				double left = ProjectX(x - 0.5, TargetZ);
				double right = ProjectX(x + 0.5, TargetZ);
				double top = ProjectY(-0.5, TargetZ);
				double bottom = ProjectY(0.5, TargetZ);

				spriteBatch.Draw(wall, new Rectangle((int) Math.Round(left), (int) Math.Round(top), (int) Math.Round(right - left), (int) Math.Round(bottom - top)), null, Color.White);
			}

			for (int z = 1; z >= 0; z--)
			{
				double targetNear = z - 0.5;
				double targetFar = z + 0.5;

				for (int x = -1; x <= 1; x++)
				{
					const double TargetY = 0.5;

					double nearY = ProjectY(TargetY, Math.Max(0, targetNear));
					double farY = ProjectY(TargetY, targetFar);

					for (int y = (int) Math.Round(nearY); y >= (int) Math.Round(farY); y--)
					{
						double wallZ = ReverseProjectY(TargetY, y);

						double left = ProjectX(x - 0.5, wallZ);
						double right = ProjectX(x + 0.5, wallZ);

						int textureOffset = (int) Math.Round((wallZ - targetNear) * wall!.Height) % wall.Height;
						spriteBatch.Draw(wall, new Rectangle((int) Math.Round(left), y, (int) Math.Round(right - left), 1), new Rectangle(0, wall.Height - 1 - textureOffset, wall.Width, 1), Color.White);
						spriteBatch.Draw(wall, new Rectangle((int) Math.Round(left), Height - 1 - y, (int) Math.Round(right - left), 1), new Rectangle(0, wall.Height - 1 - textureOffset, wall.Width, 1), Color.White);
					}
				}

				const double TargetX = -1.5;

				double nearX = ProjectX(TargetX, Math.Max(0, targetNear));
				double farX = ProjectX(TargetX, targetFar);

				for (int x = (int) Math.Round(nearX); x <= (int) Math.Round(farX); x++)
				{
					double wallZ = ReverseProjectX(TargetX, x);

					double top = ProjectY(-0.5, wallZ);
					double bottom = ProjectY(0.5, wallZ);

					int textureOffset = (int) Math.Round((wallZ - targetNear) * wall!.Height) % wall.Height;
					spriteBatch.Draw(wall, new Rectangle(x, (int) Math.Round(top), 1, (int) Math.Round(bottom - top)), new Rectangle(textureOffset, 0, 1, wall.Height), Color.White);
					spriteBatch.Draw(wall, new Rectangle(Width - 1 - x, (int) Math.Round(top), 1, (int) Math.Round(bottom - top)), new Rectangle(textureOffset, 0, 1, wall.Height), Color.White);
				}
			}

			spriteBatch.End();
			base.Draw(gameTime);
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			wall = Texture2D.FromFile(GraphicsDevice, "Wall.jpg");
		}

		private double ProjectX(double x, double z) => Width / 2.0 + x * ViewingDistance / (ViewingDistance + z) * Height;

		private double ProjectY(double y, double z) => Height / 2.0 + y * ViewingDistance / (ViewingDistance + z) * Height;

		private double ReverseProjectX(double x, double projectedX) => x * ViewingDistance * Height / (projectedX - Width / 2.0) - ViewingDistance;

		private double ReverseProjectY(double y, double projectedY) => y * ViewingDistance * Height / (projectedY - Height / 2.0) - ViewingDistance;

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			base.Update(gameTime);
		}
	}
}