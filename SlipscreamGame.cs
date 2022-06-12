using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Slipscream
{
	public class SlipscreamGame : Game
	{
		private const double ViewingDistance = 0.5;

		private Dot? dot;
		private Font? font;
		private double positionX;
		private double positionZ;
		private Texture2D? road;
		private double speed;
		private SpriteBatch? spriteBatch;

		private int Height => GraphicsDevice.PresentationParameters.BackBufferHeight;
		private int Width => GraphicsDevice.PresentationParameters.BackBufferWidth;

		public SlipscreamGame()
		{
			_ = new GraphicsDeviceManager(this);
			IsFixedTimeStep = false;
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			spriteBatch!.Begin(blendState: BlendState.NonPremultiplied);

			double deltaBend = 0;
			double bendX = 0;

			double deltaHill = 0;
			double hillY = 0;
			double lastDrawY = Height;

			for (int y = Height - 1; y >= (int) Math.Round(Height * 0.55); y--)
			{
				const double TargetY = 0.5;

				const double TargetLeft = -0.5;
				const double TargetRight = 0.5;

				const double BendFactor = 0.00003;
				const double HillFactor = 0.00002;

				double z = ReverseProjectY(TargetY, y);
				double mapZ = (z + positionZ) % 20;

				deltaBend += mapZ is >= 4 and < 6 or >= 10 and < 14 ? BendFactor
					: mapZ is >= 8 and < 10 or >= 15 and < 16 ? -BendFactor
					: 0;
				bendX += deltaBend;

				deltaHill += mapZ is >= 3 and < 5 ? HillFactor
					: mapZ is >= 13 and < 16 ? -HillFactor * 5
					: 0;
				hillY += deltaHill;

				int drawY = (int) Math.Round(y + hillY * Height);

				if (drawY >= lastDrawY)
					continue;

				double left = ProjectX(TargetLeft + positionX, z) + bendX * Width;
				double right = ProjectX(TargetRight + positionX, z) + bendX * Width;

				int textureOffset = (int) Math.Round(mapZ * road!.Height) % road.Height;
				spriteBatch.Draw(road, new Rectangle((int) Math.Round(left), drawY, (int) Math.Round(right - left), 1), new Rectangle(0, road.Height - 1 - textureOffset, road.Width, 1), Color.White);
				lastDrawY = drawY;
			}

			dot!.Draw(spriteBatch, 300, 200, 200, 200, new Color(Color.Yellow, 0.5f));
			font!.Write(spriteBatch, 400, 300, "Hello, World!");

			spriteBatch.End();
			base.Draw(gameTime);
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			road = Texture2D.FromFile(GraphicsDevice, "Road.png");
			font = new Font(GraphicsDevice);
			dot = new Dot(GraphicsDevice);
		}

		private double ProjectX(double x, double z) => Width / 2.0 + x * ViewingDistance / (ViewingDistance + z) * Width;

		private double ReverseProjectY(double y, double projectedY) => y * ViewingDistance * Height / (projectedY - Height / 2.0) - ViewingDistance;

		protected override void Update(GameTime gameTime)
		{
			const double Acceleration = 2;
			const double TopSpeed = 3;

			const double StrafeSpeed = 2;
			const double MaxStrafe = 0.5;

			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			double seconds = gameTime.ElapsedGameTime.TotalSeconds;

			if (Keyboard.GetState().IsKeyDown(Keys.W))
				speed = Math.Min(TopSpeed, speed + seconds * Acceleration);
			else if (Keyboard.GetState().IsKeyDown(Keys.S))
				speed = Math.Max(-TopSpeed, speed - seconds * Acceleration);

			if (Keyboard.GetState().IsKeyDown(Keys.A))
				positionX = Math.Min(MaxStrafe, positionX + seconds * StrafeSpeed);
			else if (Keyboard.GetState().IsKeyDown(Keys.D))
				positionX = Math.Max(-MaxStrafe, positionX - seconds * StrafeSpeed);

			positionZ = Math.Max(0, positionZ + seconds * speed);
			base.Update(gameTime);
		}
	}
}