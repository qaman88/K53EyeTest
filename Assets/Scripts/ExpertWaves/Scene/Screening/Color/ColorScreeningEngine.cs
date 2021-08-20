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
					private float redScore;
					private float greenScore;
					private float blueScore;
					private int redTotal;
					private int greenTotal;
					private int blueTotal;
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
							IColorChoice result = IColorChoice.None;

							if (GameOver == false) {
								UnityEngine.Color item = carColors[Level];
								// red component dominant
								if (item.r > item.g && item.r > item.b) {
									result = IColorChoice.Red;
								}

								// green component dominant
								else if (item.g > item.r && item.g > item.b) {
									result = IColorChoice.Green;
								}

								// blue component dominant
								else if (item.b > item.r && item.b > item.g) {
									result = IColorChoice.Blue;
								}
							}

							return result;
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
						get => (float) Math.Round(100 * ( redScore + greenScore + blueScore ) / Total);
					}

					public float RedScore {
						get => (float) Math.Round(100 * redScore / RedTotal);
					}

					public float GreenScore {
						get => (float) Math.Round(100 * greenScore / GreenTotal);
					}

					public float BlueScore {
						get => (float) Math.Round(100 * blueScore / BlueTotal);
					}

					public int Total {
						get => carColors.Count;
					}

					public int RedTotal {
						get => redTotal;
					}

					public int GreenTotal {
						get => greenTotal;
					}

					public int BlueTotal {
						get => blueTotal;
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
						GenerateCarColors();
						Level = 0;
						CurrentColor = carColors[Level];
						GameOver = Constant.Negative;
						redScore = 0;
						greenScore = 0;
						blueScore = 0;
					}

					public void NextLevel(IColorChoice color) {
						if (color == Answer) {
							switch (color) {
								case IColorChoice.Red:
									++redScore;
									break;
								case IColorChoice.Green:
									++greenScore;
									break;
								case IColorChoice.Blue:
									++blueScore;
									break;
							}

							Level++;
							if (Level <= MaxLevel) {
								Color = carColors[Level];
							}
							else {
								GameOver = true;
							}
						}
					}
					#endregion

					#region Private Functions
					private float RandomColorComponent(int start = 0, int end = 255) {
						return 1.0f * randomGenerator.Next(start, end) / maxPixel;
					}

					private void GenerateCarColors() {
						// reset colors
						carColors = new List<UnityEngine.Color>();

						// generate 3 primary colors 
						carColors.Add(UnityEngine.Color.red);
						carColors.Add(UnityEngine.Color.blue);
						carColors.Add(UnityEngine.Color.green);
						redTotal = 1;
						greenTotal = 1;
						blueTotal = 1;

						int N = 10;
						int dominent = (int) ( maxPixel / 1.5 );
						float step = maxPixel / (4 * N);

						// Generate (3 * N - 3) random colors with dominant component
						for (int n = 0, rand = 0 ; n < 3 * N - 3 ; n++, rand = randomGenerator.Next(0, 3)) {
							// Red dominated color. Fill only red color, if green and blue are full.
							if (RedTotal < N && ( rand == 0 || blueTotal >= N && greenTotal >= N )) {
								++redTotal;
								carColors.Add(new UnityEngine.Color(
									RandomColorComponent(dominent, maxPixel),
									RandomColorComponent(minPixel, dominent),
									RandomColorComponent(minPixel, dominent),
									( maxPixel - n * step ) / maxPixel
								));
							}
							else {
								// Fill only green, if blue is full. Else generate rand to be 1 or 2 in case rand is 0.
								rand = blueTotal >= N ? 1 : randomGenerator.Next(1, 3);

								// Green dominated color
								if (GreenTotal < N && rand == 1) {
									++greenTotal;
									carColors.Add(new UnityEngine.Color(
										RandomColorComponent(minPixel, dominent),
										RandomColorComponent(dominent, maxPixel),
										RandomColorComponent(minPixel, dominent),
										( maxPixel - n * step ) / maxPixel
									));
								}

								// Blue dominated color
								else if (blueTotal < N) {
									++blueTotal;
									carColors.Add(new UnityEngine.Color(
										RandomColorComponent(minPixel, dominent),
										RandomColorComponent(minPixel, dominent),
										RandomColorComponent(dominent, maxPixel),
										( maxPixel - n * step ) / maxPixel
									));
								}
							}
						}
					}
					#endregion
				}
			}
		}
	}
}
