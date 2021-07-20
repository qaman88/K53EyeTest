using ExpertWaves.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpertWaves {
	namespace Scene {
		namespace Screening {
			namespace Color {
				public class ColorScreeningEngine {
					#region Private Variables
					private List<UnityEngine.Color> carColors;
					private UnityEngine.Color color;
					private readonly Random randomGenerator = new Random();
					private int minPixel = 0;
					private int maxPixel = 255;
					private int level = 0;
					private bool gameOver =  Constant.Negative;
					private float score = 0;
					#endregion

					#region Public Variables Properties
					public UnityEngine.Color CurrentColor {
						get => Color;
						set => Color = value;
					}

					public int Level {
						get => level;
						set => level = value;
					}

					public int MaxLevel {
						get => carColors.Count - 1;
					}

					public IColorChoice Answer {
						get {
							UnityEngine.Color item = carColors[Level];
							if (item.r > item.g && item.r > item.b) {
								return IColorChoice.Red;
							}
							else if (item.g > item.r && item.g > item.b) {
								return IColorChoice.Green;
							}
							else {
								return IColorChoice.Blue;
							}
						}
					}

					public UnityEngine.Color Color {
						get => color;
						set => color = value;
					}

					public bool GameOver {
						get => gameOver;
						set => gameOver = value;
					}

					public float Score {
						get => score;
						set => score = value;
					}
					#endregion

					#region Public Functions
					public ColorScreeningEngine() {
						Level = 0;
						carColors = new List<UnityEngine.Color>();
						GenerateCarColors();
						CurrentColor = carColors[Level];
					}

					public void Restart() {
						Score = 0;
						Level = 0;
						CurrentColor = carColors[Level];
						GameOver = Constant.Negative;
					}

					public void NextLevel() {
						if (Level < carColors.Count - 1) {
							Level++;
							Color = carColors[Level];
							Score = (float) Math.Round((decimal) ( 100 * Level / ( carColors.Count - 1 ) ), 2);
						}
						else {
							GameOver = Constant.Positive;
						}
					}
					#endregion

					#region Private Functions
					private float RandomColorComponent(int start = 0, int end = 255) {
						return 1.0f * randomGenerator.Next(start, end) / maxPixel;
					}

					/**
					 * Generate N + N + N + 3 colors
					*/
					private void GenerateCarColors() {
						int N = 9;
						int dominent = maxPixel / 2;

						List<UnityEngine.Color> list = new List<UnityEngine.Color>();

						// Generate N + N + N random colors with dominant component
						for (int n = 0 ; n < N ; n++) {
							// red dominant color
							list.Add(new UnityEngine.Color(
								RandomColorComponent(dominent, maxPixel),
								 RandomColorComponent(minPixel, dominent),
								RandomColorComponent(minPixel, dominent),
								RandomColorComponent(maxPixel / 2, maxPixel)
							));

							// green dominant color
							list.Add(new UnityEngine.Color(
								RandomColorComponent(minPixel, dominent),
								RandomColorComponent(dominent, maxPixel),
								RandomColorComponent(minPixel, dominent),
								RandomColorComponent(maxPixel / 2, maxPixel)
							));

							// blue dominant color
							list.Add(new UnityEngine.Color(
								RandomColorComponent(minPixel, dominent),
								RandomColorComponent(minPixel, dominent),
								RandomColorComponent(dominent, maxPixel),
								RandomColorComponent(maxPixel / 2, maxPixel)
							));
						}

						// shuffle the car colors list
						Random rand = new Random();
						carColors = list.OrderBy(item => rand.Next()).ToList();

						// generate 3 primary colors 
						carColors.Insert(0, UnityEngine.Color.red);
						carColors.Insert(1, UnityEngine.Color.green);
						carColors.Insert(2, UnityEngine.Color.blue);
					}
					#endregion
				}
			}
		}
	}
}