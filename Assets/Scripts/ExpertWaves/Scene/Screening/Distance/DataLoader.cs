namespace ExpertWaves {
	namespace Scene {
		namespace Screening {
			namespace Distance {
				namespace Data {
					public class DataLoader {
						private readonly float[] levels = new float[]
						{
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

						public float[] Levels => this.levels;
					}
				}
			}
		}
	}
}