using System;
using UnityEngine;

namespace ExpertWaves {
	namespace Scene {
		namespace Screening {
			namespace Distance {
				public class DistanceScreening : MonoBehaviour {
					#region Variables
					private System.Random randomGenerator;
					private double score;
					private int level;
					private float angle;
					private IDirection direction;
					private bool gameover = false;
					private readonly float[] levels = new Screening.Distance.Data.DataLoader().Levels;
					#endregion

					#region Properties
					public double Score {
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

					public bool getGameOver() {
						return this.gameover;
					}

					public void setGameOver(bool state) {
						this.gameover = state;
					}

					public float OptotypeScale {
						get => 1.0f - ( this.Levels[this.level] / 100 );
					}

					public float[] Levels => this.levels;
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
								this.Direction = IDirection.RIGHT;
								break;
							case 90: // 90
								this.Direction = IDirection.UP;
								break;
							case 180: // 180
								this.Direction = IDirection.LEFT;
								break;
							case 270: // 270
								this.Direction = IDirection.DOWN;
								break;
							default:
								break;
						}
					}
					#endregion

					#region Public Functions
					public DistanceScreening() {
						this.randomGenerator = new System.Random();
						this.Reset();
					}

					public void Reset() {
						this.gameover = false;
						this.level = 0;
						this.Score = 0;
						this.Angle = 0;
						this.Direction = IDirection.RIGHT;
					}

					public void NextLevel() {
						if (this.level < this.Levels.Length - 1) {
							this.RandomAngle();
							this.level += 1;
							this.score = (float) Math.Round((decimal) ( 100 * this.level / ( this.Levels.Length - 1 ) ), 2);
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