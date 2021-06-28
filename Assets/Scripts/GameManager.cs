using System;

namespace ExpertWaves
{
	public class GameManager
	{
		private Random randomGenerator;

		private double score;
		public double Score
		{
			get => this.score;
			set => this.score = value;
		}

		private float angle;
		public float Angle
		{
			get => this.angle;
			set => this.angle = value;
		}

		private DIRECTION direction;
		public DIRECTION Direction
		{
			get => this.direction;
			set => this.direction = value;
		}

		private bool gameover = false;
		public bool getGameOver()
		{
			return this.gameover;
		}

		public void setGameOver(bool state)
		{
			this.gameover = state;
		}

		private int level;
		private readonly float[] levels = new DataManager().getGameLevels();
		public float OptotypeScale
		{
			get => 1.0f - ( this.levels[this.level] / 100 );
		}

		public GameManager()
		{
			this.randomGenerator = new Random();
			this.Reset();
		}

		public void Reset()
		{
			this.gameover = false;
			this.level = 0;
			this.Score = 0;
			this.Angle = 0;
			this.Direction = DIRECTION.RIGHT;
		}
		public void NextLevel()
		{
			if (this.level < this.levels.Length - 1) {
				this.RandomAngle();
				this.level += 1;
			}
		}

		public void setCurrentOptotypeSize(float x, float y)
		{
			float size = x * y;
			Console.WriteLine($"Current Optotype Size: {size}");
		}

		private void RandomAngle()
		{
			float rightAngle = 90;
			int scaler = randomGenerator.Next(0, 4);
			float newAngle = scaler * rightAngle;

			if (newAngle == this.Angle) {
				newAngle = ( newAngle + rightAngle ) % 360;
			}

			this.Angle = newAngle;

			switch (newAngle) {
				case 0: //  0
					this.Direction = DIRECTION.RIGHT;
					break;
				case 90: // 90
					this.Direction = DIRECTION.UP;
					break;
				case 180: // 180
					this.Direction = DIRECTION.LEFT;
					break;
				case 270: // 270
					this.Direction = DIRECTION.DOWN;
					break;
				default:
					break;
			}
		}
	}
}
