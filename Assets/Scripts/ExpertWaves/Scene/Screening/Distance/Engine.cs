using ExpertWaves.Enum;
using System;

namespace ExpertWaves {
	namespace Scene {
		namespace Screening {
			namespace Distance {
				public class Engine {
					#region Private Variables 
					private readonly float[] levels = new float[] {
							0,
							5,
							10,
							20,
							30,
							40,
							52,
							60,
							70,
							80,
							85,
							90,
							95,
							96.0f,
							96.5f,
							97.0f,
							97.5f,
							98.0f,
							98.5f,
							99.0f,
							99.5f,
							99.75f
						};
					private Random randomGenerator;
					private float score;
					private int level;
					private float angle;
					private IDirection direction = IDirection.Right;
					private bool gameover = false;
					#endregion

					#region Properties
					public float Score {
						get => this.score;
						set => this.score = value;
					}

					public float Angle {
						get => this.angle;
						set => this.angle = value;
					}

					public IDirection Direction {
						get => this.direction;
						set => this.direction = value;
					}

					public float OptotypeScale {
						get => 1.0f - ( this.Levels[this.Level] / 100 );
					}

					public float[] Levels => this.levels;

					public bool Gameover {
						get => this.gameover;
						set => this.gameover = value;
					}

					public int Level {
						get => this.level;
						set => this.level = value;
					}
					#endregion

					#region Private Functions
					private void RandomAngle() {
						float rightAngle = 90;
						int scaler = randomGenerator.Next(0, 4);
						float newAngle = scaler * rightAngle;

						if (newAngle == this.Angle) {
							newAngle = ( newAngle + rightAngle ) % 360;
						}

						this.Angle = newAngle;

						switch (newAngle) {
							case 0: //  0
								this.Direction = IDirection.Right;
								break;
							case 90: // 90
								this.Direction = IDirection.Up;
								break;
							case 180: // 180
								this.Direction = IDirection.Left;
								break;
							case 270: // 270
								this.Direction = IDirection.Down;
								break;
							default:
								break;
						}
					}
					#endregion

					#region Public Functions
					public Engine() {
						this.randomGenerator = new System.Random();
						this.Restart();
					}

					public void Restart() {
						this.Gameover = false;
						this.Level = 0;
						this.Score = 0;
						this.Angle = 0;
						this.Direction = IDirection.Right;
					}

					public void NextLevel() {
						if (this.Level < this.Levels.Length - 1) {
							this.RandomAngle();
							this.Level += 1;
							this.Score = (float) Math.Round((decimal) ( 100 * this.Level / ( this.Levels.Length - 1 ) ), 2);
						}
					}

					public void setCurrentOptotypeSize(float x, float y) {
						float size = x * y;
						Console.WriteLine($"Current Optotype Size: {size}");
					}

					#endregion
				}
			}
		}
	}
}